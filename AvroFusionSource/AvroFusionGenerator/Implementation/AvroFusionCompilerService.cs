using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The compiler service.
/// </summary>

public class AvroFusionCompilerService : IAvroFusionCompilerService
{
    private readonly IAvroFusionDynamicAssemblyGenerator _avroFusionDynamicAssemblyGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroFusionCompilerService"/> class.
    /// </summary>
    /// <param name="avroFusionDynamicAssemblyGenerator">The dynamic assembly generator.</param>
    public AvroFusionCompilerService(IAvroFusionDynamicAssemblyGenerator avroFusionDynamicAssemblyGenerator)
    {
        _avroFusionDynamicAssemblyGenerator = avroFusionDynamicAssemblyGenerator;
    }

    /// <summary>
    /// Loads the types from source.
    /// </summary>
    /// <param name="sourceDirectoryPath"></param>
    /// <param name="parentClassModelName"></param>
    /// <returns>A list of Types.</returns>
    public List<Type> LoadDotNetTypesFromSource(string sourceDirectoryPath,string parentClassModelName)
    {
        if (string.IsNullOrWhiteSpace(sourceDirectoryPath))
        {
            throw new ArgumentException("Source directory path cannot be null or whitespace.", nameof(sourceDirectoryPath));
        }


        if (string.IsNullOrWhiteSpace(parentClassModelName))
        {
            throw new ArgumentException("Parent class model name cannot be null or whitespace.", nameof(parentClassModelName));
        }
        return _avroFusionDynamicAssemblyGenerator.GenerateDynamicAssembly(sourceDirectoryPath, parentClassModelName);
    }
}