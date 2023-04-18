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
    public List<Type> GenerateDynamicAssembly(string sourceCode)
    {
        var syntaxTree = ParseSyntaxTree(sourceCode);
        var referencedAssemblies = GetReferencedAssemblies(syntaxTree);
        var compilation = CreateCompilation(syntaxTree, referencedAssemblies);
        var outputStream = EmitCompilation(compilation);
        var generatedAssembly = LoadAssembly(outputStream);

        return generatedAssembly.GetExportedTypes().ToList();
    }

    private static SyntaxTree ParseSyntaxTree(string sourceCode)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        syntaxTree = AddMissingUsingDirective(syntaxTree, "System");
        syntaxTree = AddMissingUsingDirective(syntaxTree, "System.Collections.Generic");

        return syntaxTree;
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

        var referencedAssemblies = new List<MetadataReference>();

        foreach (var assembly in assemblies)
        {
            var reference = MetadataReference.CreateFromFile(assembly.Location);
            referencedAssemblies.Add(reference);
        }

        return referencedAssemblies;
    }

    private static List<PortableExecutableReference> GetMetadataReferences()
    {
        var assemblies = new HashSet<Assembly>
        {
            typeof(object).Assembly, // System.Private.CoreLib
            typeof(Dictionary<,>).Assembly, // System.Collections
            typeof(List<>).Assembly, // System.Collections
            typeof(System.ComponentModel.DataAnnotations.ValidationAttribute).Assembly, // System.ComponentModel.Annotations
            typeof(JsonConvert).Assembly, // Newtonsoft.Json
            typeof(System.Xml.XmlReader).Assembly, // System.Xml.ReaderWriter
            typeof(System.Data.DataSet).Assembly, // System.Data.Common
        };

        var references = new List<PortableExecutableReference>();
        foreach (var assembly in assemblies)
        {
            var reference = MetadataReference.CreateFromFile(assembly.Location);
            references.Add(reference);
        }

        return references;
    }

    //private static List<MetadataReference> GetReferencedAssemblies(SyntaxTree syntaxTree)
    //{
    //    var referencedAssemblies = new List<MetadataReference>
    //    {
    //        MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
    //        MetadataReference.CreateFromFile(typeof(Dictionary<,>).GetTypeInfo().Assembly.Location),
    //        MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location),
    //        MetadataReference.CreateFromFile(typeof(List<>).GetTypeInfo().Assembly.Location),
    //        MetadataReference.CreateFromFile(typeof(DateTime).GetTypeInfo().Assembly.Location),
    //        MetadataReference.CreateFromFile(typeof(decimal).GetTypeInfo().Assembly.Location),
    //        MetadataReference.CreateFromFile(typeof(AvroUnionTypeAttribute).Assembly.Location),
    //        MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location),

    //        MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // Base libraries

    //    };

    //    var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()
    //        .Select(uds => uds.Name.ToString()).ToList();
    //    if (!usingDirectives.Contains("System"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location));
    //    if (!usingDirectives.Contains("System.Collections.Generic"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(List<>).GetTypeInfo().Assembly.Location));
    //    if (!usingDirectives.Contains("System.Collections.Generic"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(Dictionary<,>).GetTypeInfo().Assembly.Location));
    //    if (!usingDirectives.Contains("System.Collections.Generic"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(HashSet<>).GetTypeInfo().Assembly.Location));
    //    if (!usingDirectives.Contains("System.Collections.Generic"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(IEnumerable<>).GetTypeInfo().Assembly.Location));
    //    if (!usingDirectives.Contains("System.Collections.Generic"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(IList<>).GetTypeInfo().Assembly.Location));
    //    if (!usingDirectives.Contains("System.Linq"))
    //        referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(IQueryable<>).GetTypeInfo().Assembly.Location));
    //    return referencedAssemblies;
    //}

    //private static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree, List<MetadataReference> referencedAssemblies)
    //{
    //    var assemblyName = $"DynamicAssembly{Guid.NewGuid()}";
    //    return CSharpCompilation.Create(
    //        assemblyName,
    //        new[] { syntaxTree },
    //        referencedAssemblies,
    //        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    //    );
    //}

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

    private static Type FindTypeInReferencedAssemblies(string typeName, AssemblyLoadContext alc)
    {
        foreach (var assembly in alc.Assemblies)
        {
            var type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }
        return null;
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

        foreach (var typeName in missingTypes)
        {
            var type = FindTypeInReferencedAssemblies(typeName, alc);
            if (type != null)
            {
                var namespaceName = type.Namespace;
                var directive = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName));

                // Check if the namespace is already present in the using directives
                if (!usingDirectives.Any(ud => ud.Name.ToString() == namespaceName))
                {
                    usingDirectives = usingDirectives.Add(directive);
                }
            }
        }

        // Replace the compilation unit's using directives with the updated list
        compilationUnit = compilationUnit.WithUsings(usingDirectives);
        return compilationUnit.SyntaxTree;
    }
    private static AssemblyLoadContext CreateReferenceAssemblyLoadContext(List<MetadataReference> referencedAssemblies)
    {
        return new ReferenceAssemblyLoadContext(referencedAssemblies);
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

}