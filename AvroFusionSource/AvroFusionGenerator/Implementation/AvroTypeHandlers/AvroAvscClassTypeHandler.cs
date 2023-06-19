using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc class type handler.
/// </summary>

public class AvroAvscClassTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroFusionSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroAvscClassTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroAvscClassTypeHandler(Lazy<IAvroFusionSchemaGenerator> avroSchemaGenerator)
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
        if (type == null || !type.IsClass) return false;

        var knownTypes = new List<Type>
        {
            typeof(string),
            typeof(object),
            typeof(Exception),
            typeof(Uri),
            typeof(Stream),
            typeof(Task),
            typeof(ValueTask),
            typeof(CancellationToken),
            typeof(TimeSpan),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Encoding),
            typeof(MemoryStream),
            typeof(WebRequest),
            typeof(WebResponse),
            typeof(HttpRequestMessage),
            typeof(HttpResponseMessage),
            typeof(IQueryable),
            typeof(IDbConnection),
            typeof(IDbCommand),
            typeof(IDbTransaction),
            typeof(IEnumerable<>)
        };

        return !knownTypes.Contains(type) && !IsTupleType(type);
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        if (type?.FullName != null && forAvroAvscGeneratedTypes.Contains(type.FullName))
            return type.FullName;

        if (type?.FullName != null) forAvroAvscGeneratedTypes.Add(type.FullName);

        var avroFieldInfo = type?.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType))
            .Select(prop => new
            {
                name = prop.Name,
                type = IsTupleType(prop.PropertyType) ? GetTupleSchema(prop.PropertyType) : GenerateUnionTypeIfRequired(prop, forAvroAvscGeneratedTypes),
                aliasAttribute = prop.GetCustomAttribute<AvroDuplicateFieldAliasAttribute>()
            });

        if (avroFieldInfo != null)
        {
            var createAvroAvscType = new Dictionary<string, object?>
            {
                {"type", "record"},
                {"name", type?.Name},
                {"namespace", type?.Namespace},
                {
                    "fields", avroFieldInfo.Select(fieldInfo =>
                    {
                        var field = new Dictionary<string, object?>
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

        return null;
    }

    /// <summary>
    /// Are the ignored type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    private static bool IsIgnoredType(Type type)
    {
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType", StringComparison.Ordinal) ||
               (type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) &&
                type.GetGenericArguments()[0] != typeof(string));
    }

    private object GetTupleSchema(Type tupleType)
    {
        var genericArgs = tupleType.GetGenericArguments();
        var fieldNames = Enumerable.Range(1, genericArgs.Length).Select(i => $"Item{i}").ToArray();

        var schema = new Dictionary<string, object>
        {
            {"type", "record"},
            {"name", $"Tuple{genericArgs.Length}"},
            {"namespace", "CustomNamespace"}, // Change "CustomNamespace" to a namespace of your choice
            {
                "fields", genericArgs.Select((arg, i) => new Dictionary<string, object?>
                {
                    {"name", fieldNames[i]},
                    {"type", _avroSchemaGenerator.Value.GenerateAvroFusionAvscAvroType(arg, new HashSet<string>())}
                }).ToList()
            }
        };

        return schema;
    }

    /// <summary>
    /// Generates the union type if required.
    /// </summary>
    /// <param name="prop">The prop.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object.</returns>
    private object? GenerateUnionTypeIfRequired(PropertyInfo prop, HashSet<string> generatedTypes)
    {
        var avroUnionAttribute = prop.GetCustomAttribute<AvroUnionTypeAttribute>();

        if (avroUnionAttribute != null)
        {
            List<object?> unionTypes = avroUnionAttribute.UnionTypes.Select(unionType =>
                _avroSchemaGenerator.Value.GenerateAvroFusionAvscAvroType(unionType, generatedTypes)).ToList();
            unionTypes.Add("null");
            return unionTypes;
        }

        return _avroSchemaGenerator.Value.GenerateAvroFusionAvscAvroType(prop.PropertyType, generatedTypes);
    }

    private static bool IsTupleType(Type? type)
    {
        if (type == null) return false;

        if (!type.IsGenericType) return false;

        var genericTypeDefinition = type.GetGenericTypeDefinition();

        return genericTypeDefinition == typeof(ValueTuple<>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,,,>)||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,,,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,>) ||
               genericTypeDefinition.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,,>);
    }

}