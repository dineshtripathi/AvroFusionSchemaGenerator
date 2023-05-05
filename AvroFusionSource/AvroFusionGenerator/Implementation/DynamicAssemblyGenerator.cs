using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The dynamic assembly generator.
/// </summary>

public class DynamicAssemblyGenerator : IDynamicAssemblyGenerator
{
    private static List<string> DefaultUsingDirectives => new()
    {
        "System",
        "System.Collections.Generic",
        "System.Linq",
        "System.Text",
        "System.IO",
        "System.Threading.Tasks",
        "System.ComponentModel.DataAnnotations",
        "Newtonsoft.Json",
        "System.Xml.Serialization"
    };


    /// <summary>
    /// Generates the dynamic assembly.
    /// </summary>
    /// <param name="sourceDirectory">The source code directory.</param>
    /// <param name="parentClassModelName">The main class name.</param>
    /// <returns>A list of Types.</returns>
    public List<Type> GenerateDynamicAssembly(string sourceDirectory,string parentClassModelName)
    {
        var sourceFilePaths = Directory.GetFiles(sourceDirectory, "*.cs", SearchOption.AllDirectories);
        var syntaxTrees = new List<SyntaxTree>();

        foreach (var sourceFilePath in sourceFilePaths)
        {
            var fileContent = File.ReadAllText(sourceFilePath);
            var syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
            syntaxTrees.Add(syntaxTree);
        }

        var mainClassSyntaxTree = syntaxTrees.FirstOrDefault(st =>
        {
            var root = st.GetRoot();
            return root.DescendantNodes().OfType<ClassDeclarationSyntax>().Any(n => n.Identifier.ValueText == parentClassModelName);
        });

        if (mainClassSyntaxTree == null)
        {
            throw new FileNotFoundException($"The main class '{parentClassModelName}' was not found in the source directory.");
        }

        syntaxTrees = RemoveDuplicateAssemblyAttributes(syntaxTrees);

        var referencedAssemblies = GetReferencedAssemblies(syntaxTrees);
        var compilation = CreateCompilation(syntaxTrees, referencedAssemblies);
        var outputStream = EmitCompilation(compilation);
        var generatedAssembly = LoadAssembly(outputStream);

        return generatedAssembly.GetExportedTypes().ToList();


    }

    private static List<SyntaxTree> RemoveDuplicateAssemblyAttributes(List<SyntaxTree> syntaxTrees)
    {
        var newSyntaxTrees = new List<SyntaxTree>();

        foreach (var syntaxTree in syntaxTrees)
        {
            var root = syntaxTree.GetRoot();
            var newRoot = root.RemoveNodes(
                root.DescendantNodes().OfType<AttributeListSyntax>()
                    .Where(al => al.Target?.Identifier.Kind() == SyntaxKind.AssemblyKeyword), SyntaxRemoveOptions.KeepNoTrivia);
            var newSyntaxTree = syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
            newSyntaxTrees.Add(newSyntaxTree);
        }

        return newSyntaxTrees;
    }

    private static string RemoveAssemblyAttributes(string sourceCode)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceCode);
        var root = tree.GetRoot();
        var assemblyAttributes = root.DescendantNodes()
            .OfType<AttributeSyntax>()
            .Where(a => a.Name.ToString().StartsWith("assembly:"))
            .ToList();

        var newRoot = root.RemoveNodes(assemblyAttributes, SyntaxRemoveOptions.KeepNoTrivia);
        return newRoot.ToFullString();
    }
    /// <summary>
    /// Parses the syntax tree.
    /// </summary>
    /// <param name="sourceCode">The source code.</param>
    /// <param name="usingDirectives">The using directives.</param>
    /// <returns>A SyntaxTree.</returns>
    private static SyntaxTree? ParseSyntaxTree(string sourceCode, IEnumerable<string> usingDirectives)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        return usingDirectives.Aggregate(syntaxTree, (tree, s) => AddMissingUsingDirective(tree, s)!);
    }

    /// <summary>
    /// Adds the missing using directive.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="namespaceName">The namespace name.</param>
    /// <returns>A SyntaxTree.</returns>
    private static SyntaxTree? AddMissingUsingDirective(SyntaxTree syntaxTree, string namespaceName)
    {
        var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
        var usingDirectives = root?.Usings.Select(uds => uds.Name.ToString()).ToList();

        if (usingDirectives != null && !usingDirectives.Contains(namespaceName))
        {
            var newUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName));
            root = root?.AddUsings(newUsingDirective);
        }

        if (root != null) return syntaxTree.WithRootAndOptions(root, syntaxTree.Options);
        return null;
    }

    /// <summary>
    /// Gets the referenced assemblies.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <returns>A list of MetadataReferences.</returns>
    private static List<MetadataReference> GetReferencedAssemblies(List<SyntaxTree?> syntaxTrees)
    {
        var assemblies = new HashSet<Assembly>
        {
            typeof(object).Assembly, // System.Private.CoreLib
            typeof(Dictionary<,>).Assembly, // System.Collections
            typeof(List<>).Assembly, // System.Collections
            typeof(Enumerable).Assembly, // System.Linq
            typeof(DateTime).Assembly, // System.Private.CoreLib
            typeof(decimal).Assembly, // System.Private.CoreLib
            typeof(AvroUnionTypeAttribute).Assembly, // Avro library
            typeof(JsonConvert).Assembly, // Newtonsoft.Json
            typeof(XmlReader).Assembly, // System.Xml.ReaderWriter
            typeof(DataSet).Assembly, // System.Data.Common
            typeof(ValidationAttribute).Assembly, // System.ComponentModel.Annotations
            typeof(XmlSerializer).Assembly,
            typeof(HttpClient).Assembly,
            Assembly.Load(
                "System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") // System.Runtime
        };


        var usingDirectives = syntaxTrees.SelectMany(st => st.GetRoot().DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(uds => uds.Name.ToString()))
            .Distinct()
            .ToList();

        if (usingDirectives != null && !usingDirectives.Contains("System")) assemblies.Add(typeof(object).Assembly);
        if (usingDirectives != null && !usingDirectives.Contains("System.Collections.Generic"))
        {
            assemblies.Add(typeof(List<>).Assembly);
            assemblies.Add(typeof(Dictionary<,>).Assembly);
            assemblies.Add(typeof(HashSet<>).Assembly);
            assemblies.Add(typeof(IEnumerable<>).Assembly);
            assemblies.Add(typeof(IList<>).Assembly);
        }

        if (usingDirectives != null && !usingDirectives.Contains("System.Linq")) assemblies.Add(typeof(IQueryable<>).Assembly);

        return assemblies.Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
            .Cast<MetadataReference>().ToList();
    }

    /// <summary>
    /// Emits the compilation.
    /// </summary>
    /// <param name="compilation">The compilation.</param>
    /// <returns>A MemoryStream.</returns>
    private static MemoryStream EmitCompilation(CSharpCompilation compilation)
    {
        var outputStream = new MemoryStream();
        var emitResult = compilation.Emit(outputStream);

        if (!emitResult.Success)
        {
            var errors = emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.ToString());
            throw new InvalidOperationException(
                $"Failed to compile the provided C# source code: {string.Join(Environment.NewLine, errors)}");
        }

        outputStream.Seek(0, SeekOrigin.Begin);
        return outputStream;
    }

    /// <summary>
    /// Loads the assembly.
    /// </summary>
    /// <param name="outputStream">The output stream.</param>
    /// <returns>An Assembly.</returns>
    private static Assembly LoadAssembly(MemoryStream outputStream)
    {
        var assemblyLoadContext = new AssemblyLoadContext(null, true);
        return assemblyLoadContext.LoadFromStream(outputStream);
    }

    /// <summary>
    /// Creates the compilation.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="referencedAssemblies">The referenced assemblies.</param>
    /// <returns>A CSharpCompilation.</returns>
    //private static CSharpCompilation CreateCompilation(SyntaxTree? syntaxTree,
    //    List<MetadataReference> referencedAssemblies)
    //{
    //    syntaxTree = AddMissingNamespaces(syntaxTree, referencedAssemblies);

    //    var assemblyName = $"DynamicAssembly{Guid.NewGuid()}";
    //    return CSharpCompilation.Create(
    //        assemblyName,
    //        new[] {syntaxTree}!,
    //        referencedAssemblies,
    //        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    //    );
    //}
    private static CSharpCompilation CreateCompilation(List<SyntaxTree> syntaxTrees, IEnumerable<MetadataReference> references)
    {
        return CSharpCompilation.Create(
            $"DynamicAssembly_{Guid.NewGuid()}",
            syntaxTrees,
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }


    /// <summary>
    /// Adds the missing namespaces.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="referencedAssemblies">The referenced assemblies.</param>
    /// <returns>A SyntaxTree.</returns>
    private static SyntaxTree? AddMissingNamespaces(SyntaxTree? syntaxTree, List<MetadataReference> referencedAssemblies)
    {
        var root = syntaxTree?.GetRoot();
        var compilationUnit = root as CompilationUnitSyntax;
        if (compilationUnit != null)
        {
            var usingDirectives = compilationUnit.Usings;

            // Define the list of missing types to find
            var missingTypes = new List<string>
            {
                "System.DateTime",
                "System.Collections.Generic.Dictionary`2",
                "System.Collections.Generic.List`1"
            };

            var alc = CreateReferenceAssemblyLoadContext(referencedAssemblies);

            foreach (var directive in from typeName in missingTypes
                     select FindTypeInReferencedAssemblies(typeName, alc)
                     into type
                     where type != null
                     select type.Namespace
                     into namespaceName
                     let directive = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName))
                     where usingDirectives.All(ud => ud.Name.ToString() != namespaceName)
                     select directive) usingDirectives = usingDirectives.Add(directive);

            // Replace the compilation unit's using directives with the updated list
            compilationUnit = compilationUnit.WithUsings(usingDirectives);
        }

        return compilationUnit?.SyntaxTree;
    }

    /// <summary>
    /// Creates the reference assembly load context.
    /// </summary>
    /// <param name="referencedAssemblies">The referenced assemblies.</param>
    /// <returns>An AssemblyLoadContext.</returns>
    private static AssemblyLoadContext CreateReferenceAssemblyLoadContext(List<MetadataReference> referencedAssemblies)
    {
        return new ReferenceAssemblyLoadContext(referencedAssemblies);
    }

    /// <summary>
    /// Finds the type in referenced assemblies.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <param name="alc">The alc.</param>
    /// <returns>A Type? .</returns>
    private static Type? FindTypeInReferencedAssemblies(string typeName, AssemblyLoadContext alc)
    {
        return alc.Assemblies.Select(assembly => assembly.GetType(typeName)).FirstOrDefault(type => type != null);
    }

    private static bool HasChildObject(Type type)
    {
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;
            if (!propertyType.IsPrimitive && !propertyType.IsEnum && propertyType != typeof(string) && propertyType != typeof(decimal))
            {
                return true;
            }
        }

        return false;
    }
}