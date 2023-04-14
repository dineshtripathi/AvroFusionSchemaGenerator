using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class ClassTypeStrategy : IAvroTypeStrategy
{
    private readonly IAvroSchemaGenerator _avroSchemaGenerator;

    public ClassTypeStrategy(IAvroSchemaGenerator avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }
    public bool CanHandle(Type type)
    {
        return type.IsClass && !type.Equals(typeof(string));
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        if (generatedTypes.Contains(type.FullName))
            return type.FullName;

        generatedTypes.Add(type.FullName);

        var fieldInfos = type.GetProperties()
            .Select(prop => new
            {
                Name = prop.Name,
                Type = _avroSchemaGenerator.GenerateAvroType(prop.PropertyType, generatedTypes)
            });

        var avroType = new Dictionary<string, object>
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

        return avroType;
    }
}