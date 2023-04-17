using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroGuidStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Guid";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes, IEnumerable<Dictionary<string, object>> fieldInfos)
    {
        return "fixed";
    }
}