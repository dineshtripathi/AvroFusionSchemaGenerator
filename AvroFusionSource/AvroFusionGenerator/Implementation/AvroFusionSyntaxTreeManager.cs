using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using System.Runtime.Loader;
using System.Xml;
using System.Xml.Serialization;
using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AvroFusionGenerator.Implementation;

public class AvroFusionSyntaxTreeManager : IAvroFusionSyntaxTreeManager
{
    public List<SyntaxTree> CreateSyntaxTrees(IEnumerable<string> sourceFilePaths)
    {
        return sourceFilePaths.Select(sourceFilePath => File.ReadAllText(sourceFilePath)).Select(fileContent => CSharpSyntaxTree.ParseText(fileContent)).ToList();
    }

    public SyntaxTree? FindMainClassSyntaxTree(List<SyntaxTree> syntaxTrees, string parentClassModelName)
    {
        var mainClassSyntaxTree = syntaxTrees.FirstOrDefault(syntaxTree =>
        {
            var root = syntaxTree.GetRoot();
            return root.DescendantNodes().OfType<ClassDeclarationSyntax>().Any(node => node.Identifier.ValueText == parentClassModelName);
        });
        return mainClassSyntaxTree;
    }

    public List<SyntaxTree> RemoveDuplicateAssemblyAttributes(List<SyntaxTree> syntaxTrees)
    {
        return (from syntaxTree in syntaxTrees
                let root = syntaxTree.GetRoot()
                let newRoot = root.RemoveNodes(root.DescendantNodes()
                        .OfType<AttributeListSyntax>()
                        .Where(al => al.Target?.Identifier.Kind() == SyntaxKind.AssemblyKeyword),
                    SyntaxRemoveOptions.KeepNoTrivia)
                select syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options)).ToList();
    }

    public SyntaxTree ParseSyntaxTree(string sourceCode, IEnumerable<string> usingDirectives)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        return usingDirectives.Aggregate(syntaxTree, (tree, s) => AddMissingUsingDirective(tree, s)!);
    }

    public SyntaxTree? AddMissingUsingDirective(SyntaxTree syntaxTree, string namespaceName)
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

    public IEnumerable<MetadataReference> GetReferencedAssemblies(List<SyntaxTree> syntaxTrees)
    {
        var usingDirectives = syntaxTrees.SelectMany(st => st.GetRoot().DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(uds => uds.Name.ToString()))
            .Distinct()
            .ToList();

        var metadataReferences = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(DateTime).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(decimal).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(AvroUnionTypeAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(JsonConvert).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(XmlReader).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(DataSet).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ValidationAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(XmlSerializer).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(HttpClient).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly
                .Load("System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location)
        };

        if (usingDirectives.Contains("System.Collections.Generic"))
        {
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(HashSet<>).Assembly.Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(IList<>).Assembly.Location));
        }

        if (usingDirectives.Contains("System.Linq"))
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(IQueryable<>).Assembly.Location));

        return metadataReferences;
    }

    public CSharpCompilation CreateCompilationCode(List<SyntaxTree> syntaxTrees,
        IEnumerable<MetadataReference> referencedAssemblies)
    {
        return CSharpCompilation.Create(
            $"DynamicAssembly_{Guid.NewGuid()}",
            syntaxTrees,
            referencedAssemblies,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    public MemoryStream LoadCompilationCode(CSharpCompilation compilation)
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

    public Assembly LoadRequiredAssembly(MemoryStream outputStream)
    {
        var assemblyLoadContext = new AssemblyLoadContext(null, true);
        return assemblyLoadContext.LoadFromStream(outputStream);
    }
}