using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroClassTypeStrategy : IAvroTypeStrategy
{
    private readonly AvroSchemaGenerator _avroSchemaGenerator;

    public AvroClassTypeStrategy(AvroSchemaGenerator avroSchemaGenerator)
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
        {
            return type.FullName;
        }

        generatedTypes.Add(type.FullName);

        var fields = new List<Dictionary<string, object>>();

        foreach (var propertyInfo in type.GetProperties())
        {
            var propertyType = propertyInfo.PropertyType;
            var fieldSchema = _avroSchemaGenerator.GenerateAvroType(propertyType, generatedTypes);
            var field = new Dictionary<string, object>
            {
                { "name", propertyInfo.Name },
                { "type", fieldSchema }
            };
            fields.Add(field);
        }

        return new Dictionary<string, object>
        {
            { "type", "record" },
            { "name", type.Name },
            { "namespace", type.Namespace },
            { "fields", fields }
        };
    }
}