using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class CompilerService : ICompilerService
{
    private readonly IDynamicAssemblyGenerator _dynamicAssemblyGenerator;

    public CompilerService(IDynamicAssemblyGenerator dynamicAssemblyGenerator)
    {
        _dynamicAssemblyGenerator = dynamicAssemblyGenerator;
    }

    public List<Type> LoadTypesFromSource(string sourceCode)
    {
        return _dynamicAssemblyGenerator.GenerateDynamicAssembly(sourceCode);
    }
}