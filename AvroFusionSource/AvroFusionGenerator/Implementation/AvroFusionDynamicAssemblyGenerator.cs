using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The avro fusion dynamic assembly generator.
/// </summary>

public class AvroFusionDynamicAssemblyGenerator : IAvroFusionDynamicAssemblyGenerator
{
    private readonly IAvroFusionFileReader _fileReader;
    private readonly IAvroFusionSyntaxTreeManager _syntaxTreeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroFusionDynamicAssemblyGenerator"/> class.
    /// </summary>
    /// <param name="fileReader">The file reader.</param>
    /// <param name="syntaxTreeManager">The syntax tree manager.</param>
    public AvroFusionDynamicAssemblyGenerator(
        IAvroFusionFileReader fileReader,
        IAvroFusionSyntaxTreeManager syntaxTreeManager)
    {
        _fileReader = fileReader;
        _syntaxTreeManager = syntaxTreeManager;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroFusionDynamicAssemblyGenerator"/> class.
    /// </summary>
    public AvroFusionDynamicAssemblyGenerator()
        : this(new AvroFusionFileReader(), new AvroFusionSyntaxTreeManager())
    {
    }

    /// <summary>
    /// Gets the default using directives.
    /// </summary>
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
    /// <param name="sourceDirectory">The source directory.</param>
    /// <param name="csharpModelParentClass">The csharp model parent class.</param>
    /// <returns>A list of Types.</returns>
    public List<Type> GenerateDynamicAssembly(string sourceDirectory, string csharpModelParentClass)
    {
        var sourceFilePaths = _fileReader.GetFiles(sourceDirectory, "*.cs", SearchOption.AllDirectories);
        var syntaxTrees = _syntaxTreeManager.CreateSyntaxTrees(sourceFilePaths);

        var mainClassSyntaxTree = _syntaxTreeManager.FindMainClassSyntaxTree(syntaxTrees, csharpModelParentClass);
        if (mainClassSyntaxTree == null)
        {
            throw new FileNotFoundException($"The main class '{csharpModelParentClass}' was not found in the source directory.");
        }

        syntaxTrees = _syntaxTreeManager.RemoveDuplicateAssemblyAttributes(syntaxTrees);

        var referencedAssemblies = _syntaxTreeManager.GetReferencedAssemblies(syntaxTrees);
        var compilation = _syntaxTreeManager.CreateCompilationCode(syntaxTrees, referencedAssemblies);
        var outputStream = _syntaxTreeManager.LoadCompilationCode(compilation);
        var generatedAssembly = _syntaxTreeManager.LoadRequiredAssembly(outputStream);

        return generatedAssembly.GetExportedTypes().ToList();

    }
}