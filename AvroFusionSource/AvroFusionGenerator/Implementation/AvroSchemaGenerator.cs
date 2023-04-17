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

    public string GenerateCombinedSchema(IEnumerable<Type> types, string mainClassName, ProgressReporter progressReporter)
    {
        var mainType = types.FirstOrDefault(t => t.Name == mainClassName);
        if (mainType == null)
            throw new InvalidOperationException($"Could not find the main type '{mainClassName}' in the provided types.");

        var generatedTypes = new HashSet<string>();
        var avroMainType = GenerateAvroType(mainType, generatedTypes);

        var generatedSchemas = types
            .Where(t => !t.Equals(mainType))
            .Select(t => GenerateAvroType(t, generatedTypes))
            .ToList();

        var combinedSchema = new Dictionary<string, object>
        {
            { "type", "record" },
            { "name", mainType.Name },
            { "namespace", mainType.Namespace },
            { "fields", avroMainType },
            { "types", generatedSchemas }
        };

        var serializerSettings = new JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented };
        return JsonConvert.SerializeObject(combinedSchema, serializerSettings);
    }

    public object GenerateAvroType(Type type, HashSet<string> generatedTypes)
    {
        try
        {
        var strategies = _strategyResolver.ResolveStrategies();

        foreach (var strategy in strategies)
        {
            if (strategy.CanHandle(type))
            {
                     var fieldInfos = GetFieldInfos(type, generatedTypes);
                    return strategy.CreateAvroType(type, generatedTypes, fieldInfos);
            }
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
        return type.GetProperties().Select(prop => new Dictionary<string, object>
        {
            { "Name", prop.Name },
            { "Type", GenerateAvroType(prop.PropertyType, generatedTypes) }
        });
    }


}