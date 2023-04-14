using System.Reflection;
using System.Runtime.Loader;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        return CSharpSyntaxTree.ParseText(sourceCode);
    }

    private static List<MetadataReference> GetReferencedAssemblies(SyntaxTree syntaxTree)
    {
        var referencedAssemblies = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Dictionary<,>).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(List<>).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(DateTime).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(decimal).GetTypeInfo().Assembly.Location)
        };

        var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()
            .Select(uds => uds.Name.ToString()).ToList();
        if (!usingDirectives.Contains("System"))
            referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location));
        if (!usingDirectives.Contains("System.Collections.Generic"))
            referencedAssemblies.Add(MetadataReference.CreateFromFile(typeof(List<>).GetTypeInfo().Assembly.Location));

        return referencedAssemblies;
    }

    private static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree, List<MetadataReference> referencedAssemblies)
    {
        var assemblyName = $"DynamicAssembly{Guid.NewGuid()}";
        return CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            referencedAssemblies,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
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
}