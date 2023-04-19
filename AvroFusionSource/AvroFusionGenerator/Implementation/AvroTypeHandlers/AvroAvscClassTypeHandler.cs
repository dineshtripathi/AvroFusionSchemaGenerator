using System.Reflection;
using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc class type handler.
/// </summary>

public class AvroAvscClassTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroAvscClassTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsClass && !type.Equals(typeof(string));
    }

    /// <summary>
    /// Thens the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        if (forAvroAvscGeneratedTypes.Contains(type.FullName))
            return type.FullName;

        forAvroAvscGeneratedTypes.Add(type.FullName);

        var avroFieldInfo = type.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType))
            .Select(prop => new
            {
                name = prop.Name,
                type = GenerateUnionTypeIfRequired(prop, forAvroAvscGeneratedTypes),
                aliasAttribute = prop.GetCustomAttribute<AvroDuplicateFieldAliasAttribute>()
            });

        var createAvroAvscType = new Dictionary<string, object>
        {
            {"type", "record"},
            {"name", type.Name},
            {"namespace", type.Namespace},
            {
                "fields", avroFieldInfo.Select(fieldInfo =>
                {
                    var field = new Dictionary<string, object>
                    {
                        {"name", fieldInfo.name},
                        {"type", fieldInfo.type}
                    };

                    if (fieldInfo.aliasAttribute != null)
                    {
                        field["aliases"] = new List<string> { fieldInfo.aliasAttribute.Alias };
                    }

                    return field;
                }).ToList()
            }
        };

        return createAvroAvscType;
    }

    /// <summary>
    /// Are the ignored type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    private bool IsIgnoredType(Type type)
    {
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType", StringComparison.Ordinal) ||
               (type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) &&
                type.GetGenericArguments()[0] != typeof(string));
    }

    /// <summary>
    /// Generates the union type if required.
    /// </summary>
    /// <param name="prop">The prop.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object.</returns>
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