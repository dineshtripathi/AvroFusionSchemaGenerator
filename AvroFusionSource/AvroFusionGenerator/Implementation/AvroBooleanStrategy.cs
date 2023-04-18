using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroBooleanStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Boolean";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "boolean";
    }
}