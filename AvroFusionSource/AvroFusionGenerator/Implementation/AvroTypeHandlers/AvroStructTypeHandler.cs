using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroStructTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroStructTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroStructTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
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
        return type?.Namespace != null && type is {IsValueType: true, IsEnum: false, IsPrimitive: false} && !type.Namespace.StartsWith("System.");
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object? .</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> generatedTypes)
    {
        var fieldInfos = type?.GetProperties()
            .Select(prop => new
            {
                name = prop.Name,
                type = _avroSchemaGenerator.Value.GenerateAvroAvscType(prop.PropertyType, generatedTypes)
            });

        if (fieldInfos != null)
        {
            var elementType = new Dictionary<string, object?>
            {
                { "type", "record" },
                { "name", type?.Name },
                { "namespace", type?.Namespace },
                { "fields", fieldInfos.Select(fieldInfo => new Dictionary<string, object?>
                    {
                        { "name", fieldInfo.name },
                        { "type", fieldInfo.type }
                    }).ToList()
                }
            };

            return elementType;
        }

        return null;
    }
}