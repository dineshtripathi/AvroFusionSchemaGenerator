﻿using AvroFusionGenerator.ServiceInterface;
using System.Reflection;

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

        var fieldInf = type.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType))
            .Select(prop => new
            {
                name = prop.Name,
                type = GenerateUnionTypeIfRequired(prop, generatedTypes)
            });

        var avroType = new Dictionary<string, object>
        {
            { "type", "record" },
            { "name", type.Name },
          //   {"fields",fieldInfos},
            { "namespace", type.Namespace },
            { "fields", fieldInf.Select(fieldInfo => new Dictionary<string, object>
                {
                    { "name", fieldInfo.name },
                    { "type", fieldInfo.type }
                }).ToList()
            }
        };

        return avroType;
    }
    private bool IsIgnoredType(Type type)
    {
        // Add any other types that should should be ignored here.
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType") ||
               (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) && type.GetGenericArguments()[0] != typeof(string));
    }

    private object GenerateUnionTypeIfRequired(PropertyInfo prop, HashSet<string> generatedTypes)
    {
        var avroUnionAttribute = prop.GetCustomAttribute<AvroUnionTypeAttribute>();

        if (avroUnionAttribute != null)
        {
            var unionTypes = avroUnionAttribute.UnionTypes.Select(unionType =>
                _avroSchemaGenerator.Value.GenerateAvroType(unionType, generatedTypes)).ToList();
            unionTypes.Add("null"); // You can include 'null' if you want to allow null values
            return unionTypes;
        }

        return _avroSchemaGenerator.Value.GenerateAvroType(prop.PropertyType, generatedTypes);
    }
}