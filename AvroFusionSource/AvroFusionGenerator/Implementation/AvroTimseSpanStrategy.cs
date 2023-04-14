using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroTimseSpanStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "TimeSpan";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "long";
    }
}