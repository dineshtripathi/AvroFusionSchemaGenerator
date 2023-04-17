using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroClassTypeStrategy : IAvroTypeStrategy
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroClassTypeStrategy(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }
    public bool CanHandle(Type type)
    {
        return type.IsClass && !type.Equals(typeof(string));
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes, IEnumerable<Dictionary<string, object>> fieldInfos)
    {
        if (generatedTypes.Contains(type.FullName))
            return type.FullName;

        generatedTypes.Add(type.FullName);

        //var fieldInfos = type.GetProperties()
        //    .Select(prop => new
        //    {
        //        Name = prop.Name,
        //        Type = _avroSchemaGenerator.Value.GenerateAvroType(prop.PropertyType, generatedTypes)
        //    });

        var avroType = new Dictionary<string, object>
        {
            { "type", "record" },
            { "name", type.Name },
            { "namespace", type.Namespace },
            { "fields", fieldInfos.Select(fieldInfo => new Dictionary<string, object>
                {
                    { "name", fieldInfo["Name"] },
                    { "type", fieldInfo["Type"] }
                }).ToList()
            }
        };

        return avroType;
    }
}