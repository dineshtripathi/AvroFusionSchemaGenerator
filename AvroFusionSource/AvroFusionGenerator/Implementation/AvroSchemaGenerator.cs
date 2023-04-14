using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroSchemaGenerator
{
    private readonly IEnumerable<IAvroTypeStrategy> _avroTypeStrategies;

    public AvroSchemaGenerator(IEnumerable<IAvroTypeStrategy> avroTypeStrategies)
    {
        _avroTypeStrategies = avroTypeStrategies;
    }

    public object GenerateAvroType(Type type, HashSet<string> generatedTypes)
    {
        foreach (var strategy in _avroTypeStrategies)
            if (strategy.CanHandle(type))
                return strategy.CreateAvroType(type, generatedTypes);

        throw new NotSupportedException($"The type '{type.Name}' is not supported.");
    }
}