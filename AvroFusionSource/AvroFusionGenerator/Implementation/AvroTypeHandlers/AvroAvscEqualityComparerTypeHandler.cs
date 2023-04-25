using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc equality comparer type handler.
/// </summary>

public class AvroAvscEqualityComparerTypeHandler : IAvroAvscTypeHandler
{
    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type? type)
    {
        return type is {IsGenericType: true} && type.GetGenericTypeDefinition() == typeof(IEqualityComparer<>);
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var underlyingType = type?.GetGenericArguments()[0];
        return $"IEqualityComparer<{underlyingType?.Name}>";
    }
}