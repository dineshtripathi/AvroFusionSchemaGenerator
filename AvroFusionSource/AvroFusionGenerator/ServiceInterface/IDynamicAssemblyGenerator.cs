namespace AvroFusionGenerator.ServiceInterface;

public interface IDynamicAssemblyGenerator
{
    List<Type> GenerateDynamicAssembly(string csharpModelParentClass);
}