using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroTupleTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroTupleTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroTupleTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type? type) => type is {IsGenericType: true} && (type.GetGenericTypeDefinition() == typeof(Tuple<>) || type.GetGenericTypeDefinition() == typeof(ValueTuple<>));

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object? .</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> generatedTypes)
    {
        var genericArguments = type?.GetGenericArguments();
        if (genericArguments != null)
        {
            var tupleElements = genericArguments.Select(arg => _avroSchemaGenerator.Value.GenerateAvroAvscType(arg, generatedTypes)).ToList();

            var elementType = new Dictionary<string, object?>
            {
                { "type", "tuple" },
                { "name", $"{type?.Name}_tuple" },
                { "namespace", type?.Namespace },
                { "elements", tupleElements }
            };

            return elementType;
        }

        return null;
    }
}