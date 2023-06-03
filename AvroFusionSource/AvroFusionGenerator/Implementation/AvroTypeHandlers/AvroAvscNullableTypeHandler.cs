using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc nullable type handler.
/// </summary>

public class AvroAvscNullableTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroFusionSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroAvscNullableTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroAvscNullableTypeHandler(Lazy<IAvroFusionSchemaGenerator> avroSchemaGenerator)
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
        return type is {IsGenericType: true} && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        if (type != null)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            var underlyingAvroType =
                _avroSchemaGenerator.Value.GenerateAvroFusionAvscAvroType(underlyingType, forAvroAvscGeneratedTypes);

            return new[] {"null", underlyingAvroType};
        }

        return null;
    }
}