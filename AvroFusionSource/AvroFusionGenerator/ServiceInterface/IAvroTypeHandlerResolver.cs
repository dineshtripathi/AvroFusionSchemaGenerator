namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The avro type strategy resolver.
/// </summary>

public interface IAvroTypeHandlerResolver
{
    /// <summary>
    /// Resolves the strategies.
    /// </summary>
    /// <returns>A list of IAvroAvscTypeHandlers.</returns>
    IEnumerable<IAvroAvscTypeHandler> ResolveStrategies();
}