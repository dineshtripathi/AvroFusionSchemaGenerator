using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroArrayTypeStrategy : IAvroTypeStrategy
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroArrayTypeStrategy(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool CanHandle(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes, IEnumerable<Dictionary<string, object>> fieldInfos)
    {
        var elementType = _avroSchemaGenerator.Value.GenerateAvroType(type.GetGenericArguments()[0], generatedTypes);
        return new Dictionary<string, object> { { "type", "array" }, { "items", elementType } };
    }
}