﻿using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscIDictionaryTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscIDictionaryTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
               type.GetGenericArguments()[0] == typeof(string);
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var valueType =
            _avroSchemaGenerator.Value.GenerateAvroAvscType(type.GetGenericArguments()[1], forAvroAvscGeneratedTypes);
        return new Dictionary<string, object> { { "type", "map" }, { "values", valueType } };
    }
}