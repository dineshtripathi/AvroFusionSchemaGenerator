using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscEqualityComparerTypeHandler : IAvroAvscTypeHandler
{
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEqualityComparer<>);
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var underlyingType = type.GetGenericArguments()[0];
        return $"IEqualityComparer<{underlyingType.Name}>";
    }
}