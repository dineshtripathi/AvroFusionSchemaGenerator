using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroFusionSyntaxTreeManager
{
    /// <summary>
    /// Creates the syntax trees.
    /// </summary>
    /// <param name="sourceFilePaths">The source file paths.</param>
    /// <returns>A list of SyntaxTrees.</returns>
    List<SyntaxTree> CreateSyntaxTrees(IEnumerable<string> sourceFilePaths);
    /// <summary>
    /// Finds the main class syntax tree.
    /// </summary>
    /// <param name="syntaxTrees">The syntax trees.</param>
    /// <param name="parentClassModelName">The parent class model name.</param>
    /// <returns>A     SyntaxTree? .</returns>
    SyntaxTree? FindMainClassSyntaxTree(List<SyntaxTree> syntaxTrees, string parentClassModelName);
    /// <summary>
    /// Removes the duplicate assembly attributes.
    /// </summary>
    /// <param name="syntaxTrees">The syntax trees.</param>
    /// <returns>A list of SyntaxTrees.</returns>
    List<SyntaxTree> RemoveDuplicateAssemblyAttributes(List<SyntaxTree> syntaxTrees);
    /// <summary>
    /// Parses the syntax tree.
    /// </summary>
    /// <param name="sourceCode">The source code.</param>
    /// <param name="usingDirectives">The using directives.</param>
    /// <returns>A SyntaxTree.</returns>
    SyntaxTree ParseSyntaxTree(string sourceCode, IEnumerable<string> usingDirectives);
    /// <summary>
    /// Adds the missing using directive.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="namespaceName">The namespace name.</param>
    /// <returns>A     SyntaxTree? .</returns>
    SyntaxTree? AddMissingUsingDirective(SyntaxTree syntaxTree, string namespaceName);
    /// <summary>
    /// Gets the referenced assemblies.
    /// </summary>
    /// <param name="syntaxTrees">The syntax trees.</param>
    /// <returns>A list of MetadataReferences.</returns>
    IEnumerable<MetadataReference> GetReferencedAssemblies(List<SyntaxTree> syntaxTrees);

    /// <summary>
    /// Creates the compilation code.
    /// </summary>
    /// <param name="syntaxTrees">The syntax trees.</param>
    /// <param name="referencedAssemblies">The referenced assemblies.</param>
    /// <returns>A CSharpCompilation.</returns>
    CSharpCompilation CreateCompilationCode(List<SyntaxTree> syntaxTrees,
        IEnumerable<MetadataReference> referencedAssemblies);

    /// <summary>
    /// Loads the compilation code.
    /// </summary>
    /// <param name="compilation">The compilation.</param>
    /// <returns>A MemoryStream.</returns>
    MemoryStream LoadCompilationCode(CSharpCompilation compilation);
    /// <summary>
    /// Loads the required assembly.
    /// </summary>
    /// <param name="outputStream">The output stream.</param>
    /// <returns>An Assembly.</returns>
    Assembly LoadRequiredAssembly(MemoryStream outputStream);
}