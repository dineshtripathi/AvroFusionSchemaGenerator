using AvroFusionGenerator.ServiceInterface;
using Newtonsoft.Json;

namespace AvroFusionGenerator.Implementation;

public class AvroSchemaGenerator : IAvroSchemaGenerator
{
    private readonly IAvroTypeStrategyResolver _strategyResolver;

    public AvroSchemaGenerator(IAvroTypeStrategyResolver strategyResolver)
    {
        _strategyResolver = strategyResolver;
    }

    
    public string GenerateAvroAvscSshema(IEnumerable<Type> types, string mainClassName,
        ProgressReporter progressReporter)
    {
        var mainType = types.FirstOrDefault(t => t.Name == mainClassName);
        if (mainType == null)
            throw new InvalidOperationException(
                $"Could not find the main type '{mainClassName}' in the provided types.");

        var generatedTypes = new HashSet<string>();
        var avroMainType = GenerateAvroAvscType(mainType, generatedTypes) as Dictionary<string, object>;

        var generatedSchemas = types
            .Where(t => !t.Equals(mainType))
            .Select(t => GenerateAvroAvscType(t, generatedTypes))
            .OfType<Dictionary<string, object>>()
            .ToList();

        try
        {
            var mainTypeFields = avroMainType["fields"] as IEnumerable<Dictionary<string, object>>;
            var additionalFields = generatedSchemas
                .Where(schema => schema.ContainsKey("fields"))
                .SelectMany(schema => schema["fields"] as IEnumerable<Dictionary<string, object>>)
                .ToList();

            var allFields = mainTypeFields.Concat(additionalFields).ToList();

            var combinedSchema = new Dictionary<string, object>
            {
                {"type", "record"},
                {"namespace", $"{types.First().Namespace}"},
                {"name", mainClassName},
                {"fields", allFields}
            };

            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            return JsonConvert.SerializeObject(combinedSchema, serializerSettings);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public object GenerateAvroAvscType(Type type, HashSet<string> generatedTypes)
    {
        try
        {
            var strategies = _strategyResolver.ResolveStrategies();

            foreach (var strategy in strategies)
                if (strategy.IfCanHandleAvroAvscType(type))
                {
                   // var fieldInfos = GetFieldInfos(type, generatedTypes);
                    return strategy.ThenCreateAvroAvscType(type, generatedTypes);
                }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        throw new NotSupportedException($"The type '{type.Name}' is not supported.");
    }

    private IEnumerable<Dictionary<string, object>> GetFieldInfos(Type type, HashSet<string> generatedTypes)
    {
        return type.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType)).Select(prop => new Dictionary<string, object>
        {
            {"name", prop.Name},
            {"type", GenerateAvroAvscType(prop.PropertyType, generatedTypes)}
        });
    }

    private bool IsIgnoredType(Type type)
    {
        // Add any other types that should should be ignored here.
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType") ||
               (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) && type.GetGenericArguments()[0] != typeof(string));
    }

}