using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroDoubleStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Double";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "double";
    }
}