using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroTupleTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroFusionSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroTupleTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroTupleTypeHandler(Lazy<IAvroFusionSchemaGenerator> avroSchemaGenerator)
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
            var tupleElements = genericArguments.Select(arg => _avroSchemaGenerator.Value.GenerateAvroFusionAvscAvroType(arg, generatedTypes)).ToList();

            var avroTypeName = $"{type?.Name.Replace("`", "_")}_{genericArguments.Length}_tuple";
            var elementType = new Dictionary<string, object?>
            {
                { "type", "record" },
                { "name", avroTypeName },
                { "namespace", type?.Namespace },
                { "fields", tupleElements }
            };

            return elementType;
        }

        return null;
    }
}