using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroDictionaryTypeStrategy : IAvroTypeStrategy
{
    private readonly IAvroSchemaGenerator _avroSchemaGenerator;

    public AvroDictionaryTypeStrategy(IAvroSchemaGenerator avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool CanHandle(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
               type.GetGenericArguments()[0] == typeof(string);
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        var valueType = _avroSchemaGenerator.GenerateAvroType(type.GetGenericArguments()[1], generatedTypes);
        return new Dictionary<string, object> { { "type", "map" }, { "values", valueType } };
    }
}