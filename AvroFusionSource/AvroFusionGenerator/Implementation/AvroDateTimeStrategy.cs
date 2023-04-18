using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroDateTimeStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "DateTime";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "long";
    }
}