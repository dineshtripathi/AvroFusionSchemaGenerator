using AvroFusionGenerator.ServiceInterface;
using Newtonsoft.Json;

namespace AvroFusionGenerator.Implementation;

public class AvroSchemaGenerator : IAvroSchemaGenerator
{
    private readonly IEnumerable<IAvroTypeStrategy> _avroTypeStrategies;

    public AvroSchemaGenerator(IEnumerable<IAvroTypeStrategy> avroTypeStrategies)
    {
        _avroTypeStrategies = avroTypeStrategies;
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
        foreach (var strategy in _avroTypeStrategies)
        {
            if (strategy.CanHandle(type))
            {
                return strategy.CreateAvroType(type, generatedTypes);
            }
        }

        throw new NotSupportedException($"The type '{type.Name}' is not supported.");
    }
}