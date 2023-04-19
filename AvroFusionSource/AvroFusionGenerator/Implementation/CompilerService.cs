using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The compiler service.
/// </summary>

public class CompilerService : ICompilerService
{
    private readonly IDynamicAssemblyGenerator _dynamicAssemblyGenerator;

    public CompilerService(IDynamicAssemblyGenerator dynamicAssemblyGenerator)
    {
        _dynamicAssemblyGenerator = dynamicAssemblyGenerator;
    }

    /// <summary>
    /// Loads the types from source.
    /// </summary>
    /// <param name="sourceCode">The source code.</param>
    /// <returns>A list of Types.</returns>
    public List<Type> LoadTypesFromSource(string sourceCode)
    {
        return _dynamicAssemblyGenerator.GenerateDynamicAssembly(sourceCode);
    }
}