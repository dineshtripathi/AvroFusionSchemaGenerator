using AvroFusionGenerator.ServiceInterface;
using Newtonsoft.Json;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The avro schema generator.
/// </summary>

public class AvroSchemaGenerator : IAvroSchemaGenerator
{
    private readonly IAvroTypeHandlerResolver _handlerResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroSchemaGenerator"/> class.
    /// </summary>
    /// <param name="handlerResolver">The strategy resolver.</param>
    public AvroSchemaGenerator(IAvroTypeHandlerResolver handlerResolver)
    {
        _handlerResolver = handlerResolver;
    }


    /// <summary>
    /// Generates the avro avsc sshema.
    /// </summary>
    /// <param name="types">The types.</param>
    /// <param name="mainClassName">The main class name.</param>
    /// <param name="progressReporter">The progress reporter.</param>
    /// <returns>A string.</returns>
    public string? GenerateAvroAvscSchema(IEnumerable<Type?> types, string mainClassName,
        ProgressReporter progressReporter)
    {
        var enumerable = types.ToList();
        var mainType = enumerable.FirstOrDefault(t => t?.Name == mainClassName);
        if (mainType == null)
            throw new InvalidOperationException(
                $"Could not find the main type '{mainClassName}' in the provided types.");

        var generatedTypes = new HashSet<string>();
        var avroMainType = GenerateAvroAvscType(mainType, generatedTypes) as Dictionary<string, object>;

        var generatedSchemas = enumerable
            .Where(t => t != mainType)
            .Select(t => GenerateAvroAvscType(t, generatedTypes))
            .OfType<Dictionary<string, object>>()
            .ToList();

        try
        {
            var mainTypeFields = avroMainType?["fields"] as IEnumerable<Dictionary<string, object>>;
            var additionalFields = generatedSchemas
                .Where(schema => schema.ContainsKey("fields"))
                .SelectMany(schema => (IEnumerable<Dictionary<string, object?>>)schema["fields"])
                .ToList();

            if (mainTypeFields != null)
            {
                var allFields = mainTypeFields.Concat(additionalFields!).ToList();

                var combinedSchema = new Dictionary<string, object>
                {
                    {"type", "record"},
                    {"namespace", $"{enumerable.First()?.Namespace}"},
                    {"name", mainClassName},
                    {"fields", allFields}
                };

                var serializerSettings = new JsonSerializerSettings {Formatting = Formatting.Indented};
                return JsonConvert.SerializeObject(combinedSchema, serializerSettings);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }


    /// <summary>
    /// Generates the avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object.</returns>
    public object? GenerateAvroAvscType(Type? type, HashSet<string> generatedTypes)
    {
        try
        {
            var strategies = _handlerResolver.ResolveStrategies();

            foreach (var strategy in strategies)
                if (strategy.IfCanHandleAvroAvscType(type))
                    // var fieldInfos = GetFieldInfos(type, generatedTypes);
                    return strategy.ThenCreateAvroAvscType(type, generatedTypes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        throw new NotSupportedException($"The type '{type?.Name}' is not supported.");
    }

    /// <summary>
    /// Gets the field infos.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>A list of Dictionary.</returns>
    private IEnumerable<Dictionary<string, object?>> GetFieldInfos(Type type, HashSet<string> generatedTypes)
    {
        return type.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType)).Select(prop =>
            new Dictionary<string, object?>
            {
                {"name", prop.Name},
                {"type", GenerateAvroAvscType(prop.PropertyType, generatedTypes)}
            });
    }

    /// <summary>
    /// Are the ignored type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    private static bool IsIgnoredType(Type type)
    {
        // Add any other types that should should be ignored here.
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType", StringComparison.Ordinal) ||
               (type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) &&
                type.GetGenericArguments()[0] != typeof(string));
    }
}