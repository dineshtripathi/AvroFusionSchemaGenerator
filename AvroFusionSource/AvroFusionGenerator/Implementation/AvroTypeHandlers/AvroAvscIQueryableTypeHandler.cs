using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscIQueryableTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscIQueryableTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IQueryable<>);
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var itemType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(type.GetGenericArguments()[0], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object> { { "type", "array" }, { "items", itemType } };
    }
}