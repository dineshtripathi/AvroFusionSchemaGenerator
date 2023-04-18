using AvroFusionGenerator.ServiceInterface;
using System.Reflection;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscClassTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscClassTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsClass && !type.Equals(typeof(string));
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        if (forAvroAvscGeneratedTypes.Contains(type.FullName))
            return type.FullName;

        forAvroAvscGeneratedTypes.Add(type.FullName);

        var avroFieldInfo = type.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType))
            .Select(prop => new
            {
                name = prop.Name,
                type = GenerateUnionTypeIfRequired(prop, forAvroAvscGeneratedTypes)
            });

        var createAvroAvscType = new Dictionary<string, object>
        {
            { "type", "record" },
            { "name", type.Name },
            { "namespace", type.Namespace },
            { "fields", avroFieldInfo.Select(fieldInfo => new Dictionary<string, object>
                {
                    { "name", fieldInfo.name },
                    { "type", fieldInfo.type }
                }).ToList()
            }
        };

        return createAvroAvscType;
    }
    private bool IsIgnoredType(Type type)
    {
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType", StringComparison.Ordinal) ||
               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) && type.GetGenericArguments()[0] != typeof(string);
    }

    private object GenerateUnionTypeIfRequired(PropertyInfo prop, HashSet<string> generatedTypes)
    {
        var avroUnionAttribute = prop.GetCustomAttribute<AvroUnionTypeAttribute>();

        if (avroUnionAttribute != null)
        {
            var unionTypes = avroUnionAttribute.UnionTypes.Select(unionType =>
                _avroSchemaGenerator.Value.GenerateAvroAvscType(unionType, generatedTypes)).ToList();
            unionTypes.Add("null");
            return unionTypes;
        }

        return _avroSchemaGenerator.Value.GenerateAvroAvscType(prop.PropertyType, generatedTypes);
    }
}