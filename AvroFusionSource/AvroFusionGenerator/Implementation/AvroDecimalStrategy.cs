using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroDecimalStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Decimal";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "bytes";
    }
}