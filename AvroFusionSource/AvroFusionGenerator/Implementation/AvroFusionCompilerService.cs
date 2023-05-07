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
        return _avroFusionDynamicAssemblyGenerator.GenerateDynamicAssembly(sourceDirectoryPath, parentClassModelName);
    }
}