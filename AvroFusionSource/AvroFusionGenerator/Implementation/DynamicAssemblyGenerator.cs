using System.Reflection;
using System.Runtime.Loader;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AvroFusionGenerator.Implementation;
public class DynamicAssemblyGenerator : IDynamicAssemblyGenerator
{
    private static List<string> DefaultUsingDirectives => new List<string>
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


    public List<Type> GenerateDynamicAssembly(string sourceCode)
    {
        var syntaxTree = ParseSyntaxTree(sourceCode, DefaultUsingDirectives);
        var referencedAssemblies = GetReferencedAssemblies(syntaxTree);
        var compilation = CreateCompilation(syntaxTree, referencedAssemblies);
        var outputStream = EmitCompilation(compilation);
        var generatedAssembly = LoadAssembly(outputStream);

        return generatedAssembly.GetExportedTypes().ToList();
    }

    private static SyntaxTree ParseSyntaxTree(string sourceCode, IEnumerable<string> usingDirectives)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        return usingDirectives.Aggregate(syntaxTree, (current, directive) => AddMissingUsingDirective(current, directive));
    }

    private static SyntaxTree AddMissingUsingDirective(SyntaxTree syntaxTree, string namespaceName)
    {
        var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
        var usingDirectives = root.Usings.Select(uds => uds.Name.ToString()).ToList();

        if (!usingDirectives.Contains(namespaceName))
        {
            var newUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName));
            root = root.AddUsings(newUsingDirective);
        }

        return syntaxTree.WithRootAndOptions(root, syntaxTree.Options);
    }

    private static List<MetadataReference> GetReferencedAssemblies(SyntaxTree syntaxTree)
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
            typeof(System.Xml.XmlReader).Assembly, // System.Xml.ReaderWriter
            typeof(System.Data.DataSet).Assembly, // System.Data.Common
            typeof(System.ComponentModel.DataAnnotations.ValidationAttribute).Assembly, // System.ComponentModel.Annotations
            typeof(System.Xml.Serialization.XmlSerializer).Assembly,
            Assembly.Load("System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), // System.Runtime
        };


        var usingDirectives = syntaxTree.GetRoot().DescendantNodes()
            .OfType<UsingDirectiveSyntax>()
            .Select(uds => uds.Name.ToString())
            .ToList();

        if (!usingDirectives.Contains("System"))
        {
            assemblies.Add(typeof(object).Assembly);
        }
        if (!usingDirectives.Contains("System.Collections.Generic"))
        {
            assemblies.Add(typeof(List<>).Assembly);
            assemblies.Add(typeof(Dictionary<,>).Assembly);
            assemblies.Add(typeof(HashSet<>).Assembly);
            assemblies.Add(typeof(IEnumerable<>).Assembly);
            assemblies.Add(typeof(IList<>).Assembly);
        }
        if (!usingDirectives.Contains("System.Linq"))
        {
            assemblies.Add(typeof(IQueryable<>).Assembly);
        }

        return assemblies.Select(assembly => MetadataReference.CreateFromFile(assembly.Location)).Cast<MetadataReference>().ToList();
    }

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

    private static Assembly LoadAssembly(MemoryStream outputStream)
    {
        var assemblyLoadContext = new AssemblyLoadContext(null, true);
        return assemblyLoadContext.LoadFromStream(outputStream);
    }

    private static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree, List<MetadataReference> referencedAssemblies)
    {
        syntaxTree = AddMissingNamespaces(syntaxTree, referencedAssemblies);

        var assemblyName = $"DynamicAssembly{Guid.NewGuid()}";
        return CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            referencedAssemblies,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
    }
    private static SyntaxTree AddMissingNamespaces(SyntaxTree syntaxTree, List<MetadataReference> referencedAssemblies)
    {
        var root = syntaxTree.GetRoot();
        var compilationUnit = root as CompilationUnitSyntax;
        var usingDirectives = compilationUnit.Usings;

        // Define the list of missing types to find
        var missingTypes = new List<string>
        {
            "System.DateTime",
            "System.Collections.Generic.Dictionary`2",
            "System.Collections.Generic.List`1"
        };

        var alc = CreateReferenceAssemblyLoadContext(referencedAssemblies);

        foreach (var directive in from typeName in missingTypes select FindTypeInReferencedAssemblies(typeName, alc) into type where type != null select type.Namespace into namespaceName let directive = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName)) where !usingDirectives.Any(ud => ud.Name.ToString() == namespaceName) select directive)
        {
            usingDirectives = usingDirectives.Add(directive);
        }

        // Replace the compilation unit's using directives with the updated list
        compilationUnit = compilationUnit.WithUsings(usingDirectives);
        return compilationUnit.SyntaxTree;
    }
    private static AssemblyLoadContext CreateReferenceAssemblyLoadContext(List<MetadataReference> referencedAssemblies)
    {
        return new ReferenceAssemblyLoadContext(referencedAssemblies);
    }
    private static Type? FindTypeInReferencedAssemblies(string typeName, AssemblyLoadContext alc)
    {
        return alc.Assemblies.Select(assembly => assembly.GetType(typeName)).FirstOrDefault(type => type != null);
    }
   
}