using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroUInt16Strategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "UInt16";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "int";
    }
}