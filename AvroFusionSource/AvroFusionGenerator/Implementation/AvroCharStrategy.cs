using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroCharStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Char";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "int";
    }
}