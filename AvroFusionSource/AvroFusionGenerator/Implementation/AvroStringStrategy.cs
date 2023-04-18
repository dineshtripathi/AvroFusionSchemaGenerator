using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroStringStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "String";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "string";
    }
}