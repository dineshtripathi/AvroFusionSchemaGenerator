using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscIDictionaryTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroAvscIDictionaryTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroAvscIDictionaryTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
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
        return type is {IsGenericType: true} && type.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
               type.GetGenericArguments()[0] == typeof(string);
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object? .</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var valueType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(type?.GetGenericArguments()[1], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object?> { { "type", "map" }, { "values", valueType } };
    }
}