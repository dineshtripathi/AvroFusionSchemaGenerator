using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroEqualityComparerTypeStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEqualityComparer<>);
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        var underlyingType = type.GetGenericArguments()[0];
        return $"IEqualityComparer<{underlyingType.Name}>";
    }
}