namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The avro type strategy resolver.
/// </summary>

public interface IAvroTypeStrategyResolver
{
    /// <summary>
    /// Resolves the strategies.
    /// </summary>
    /// <returns>A list of IAvroAvscTypeHandlers.</returns>
    IEnumerable<IAvroAvscTypeHandler> ResolveStrategies();
}