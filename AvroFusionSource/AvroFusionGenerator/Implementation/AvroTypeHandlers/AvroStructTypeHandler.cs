using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroStructTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroStructTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.Namespace.StartsWith("System.");
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> generatedTypes)
    {
        var fieldInfos = type.GetProperties()
            .Select(prop => new
            {
                Name = prop.Name,
                Type = _avroSchemaGenerator.Value.GenerateAvroAvscType(prop.PropertyType, generatedTypes)
            });

        var elementType = new Dictionary<string, object>
        {
            { "type", "record" },
            { "name", type.Name },
            { "namespace", type.Namespace },
            { "fields", fieldInfos.Select(fieldInfo => new Dictionary<string, object>
                {
                    { "name", fieldInfo.Name },
                    { "type", fieldInfo.Type }
                }).ToList()
            }
        };

        return elementType;
    }
}