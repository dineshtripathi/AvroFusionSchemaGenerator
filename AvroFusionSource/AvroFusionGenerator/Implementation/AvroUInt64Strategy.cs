using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroUInt64Strategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "UInt64";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "fixed";
    }
}