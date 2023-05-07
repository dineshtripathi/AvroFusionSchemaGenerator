using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroFusionSyntaxTreeManager
{
    List<SyntaxTree> CreateSyntaxTrees(IEnumerable<string> sourceFilePaths);
    SyntaxTree? FindMainClassSyntaxTree(List<SyntaxTree> syntaxTrees, string parentClassModelName);
    List<SyntaxTree> RemoveDuplicateAssemblyAttributes(List<SyntaxTree> syntaxTrees);
    SyntaxTree ParseSyntaxTree(string sourceCode, IEnumerable<string> usingDirectives);
    SyntaxTree? AddMissingUsingDirective(SyntaxTree syntaxTree, string namespaceName);
    IEnumerable<MetadataReference> GetReferencedAssemblies(List<SyntaxTree> syntaxTrees);

    CSharpCompilation CreateCompilationCode(List<SyntaxTree> syntaxTrees,
        IEnumerable<MetadataReference> referencedAssemblies);

    MemoryStream LoadCompilationCode(CSharpCompilation compilation);
    Assembly LoadRequiredAssembly(MemoryStream outputStream);
}