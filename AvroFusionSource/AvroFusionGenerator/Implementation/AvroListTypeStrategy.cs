using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroListTypeStrategy : IAvroTypeStrategy
{
    private readonly IAvroSchemaGenerator _avroSchemaGenerator;

    public AvroListTypeStrategy(IAvroSchemaGenerator avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool CanHandle(Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        var itemType = _avroSchemaGenerator.GenerateAvroType(type.GetGenericArguments()[0], generatedTypes);
        return new Dictionary<string, object> { { "type", "array" }, { "items", itemType } };
    }
}