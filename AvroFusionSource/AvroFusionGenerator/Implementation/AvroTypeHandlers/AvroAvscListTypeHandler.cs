using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc list type handler.
/// </summary>

public class AvroAvscListTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroFusionSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroAvscListTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroAvscListTypeHandler(Lazy<IAvroFusionSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type? type)
    {
        return type is {IsGenericType: true} && (type.GetGenericTypeDefinition() == typeof(List<>) ||
                                                 type.GetGenericTypeDefinition() == typeof(IList<>) ||
                                                 type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var itemType =
            _avroSchemaGenerator.Value.GenerateAvroFusionAvscAvroType(type?.GetGenericArguments()[0], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object?> {{"type", "array"}, {"items", itemType}};
    }
}