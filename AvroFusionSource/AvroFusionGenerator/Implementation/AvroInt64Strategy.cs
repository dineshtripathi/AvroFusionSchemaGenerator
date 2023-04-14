using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroInt64Strategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Int64";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "long";
    }
}