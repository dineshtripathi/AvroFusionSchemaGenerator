using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc dictionary type handler.
/// </summary>

public class AvroAvscDictionaryTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscDictionaryTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
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
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
               type.GetGenericArguments()[0] == typeof(string);
    }

    /// <summary>
    /// Thens the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var valueType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(type.GetGenericArguments()[1], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object> {{"type", "map"}, {"values", valueType}};
    }
}