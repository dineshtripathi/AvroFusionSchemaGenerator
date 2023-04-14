using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroUInt32Strategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "UInt32";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "long";
    }
}