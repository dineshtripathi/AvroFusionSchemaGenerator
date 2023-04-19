using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc array type handler.
/// </summary>

public class AvroAvscArrayTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscArrayTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    /// <summary>
    /// Thens the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var elementType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(type.GetGenericArguments()[0], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object> {{"type", "array"}, {"items", elementType}};
    }
}