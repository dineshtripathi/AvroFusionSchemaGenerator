using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc list type handler.
/// </summary>

public class AvroAvscListTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscListTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
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
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) ||
                                      type.GetGenericTypeDefinition() == typeof(IList<>) ||
                                      type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    /// <summary>
    /// Thens the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var itemType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(type.GetGenericArguments()[0], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object> {{"type", "array"}, {"items", itemType}};
    }
}