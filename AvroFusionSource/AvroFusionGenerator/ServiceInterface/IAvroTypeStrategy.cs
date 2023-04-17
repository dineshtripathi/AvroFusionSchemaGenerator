namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroTypeStrategy
{
    bool CanHandle(Type type);
    object CreateAvroType(Type type, HashSet<string> generatedTypes, IEnumerable<Dictionary<string, object>> fieldInfos);

}