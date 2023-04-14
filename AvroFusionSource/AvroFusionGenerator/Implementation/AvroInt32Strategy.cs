using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroInt32Strategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.Name == "Int32";
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return "int";
    }
}