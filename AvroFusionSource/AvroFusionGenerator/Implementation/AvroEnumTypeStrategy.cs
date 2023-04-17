using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroEnumTypeStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.IsEnum;
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes, IEnumerable<Dictionary<string, object>> fieldInfos)
    {
        var symbols = Enum.GetNames(type);
        return new Dictionary<string, object>
        {
            { "type", "enum" },
            { "name", type.Name },
            { "namespace", type.Namespace },
            { "symbols", symbols }
        };
    }
}