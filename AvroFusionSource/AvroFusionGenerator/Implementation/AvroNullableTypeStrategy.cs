using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroNullableTypeStrategy : IAvroTypeStrategy
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroNullableTypeStrategy(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool CanHandle(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        var underlyingAvroType = _avroSchemaGenerator.Value.GenerateAvroType(underlyingType, generatedTypes);

        return new object[] { "null", underlyingAvroType };
    }
}