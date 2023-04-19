using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc nullable type handler.
/// </summary>

public class AvroAvscNullableTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscNullableTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
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
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// Thens the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        var underlyingAvroType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(underlyingType, forAvroAvscGeneratedTypes);

        return new[] {"null", underlyingAvroType};
    }
}