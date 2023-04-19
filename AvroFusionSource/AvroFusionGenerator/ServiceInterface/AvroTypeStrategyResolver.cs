using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The avro type strategy resolver.
/// </summary>

public class AvroTypeStrategyResolver : IAvroTypeStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public AvroTypeStrategyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Resolves the strategies.
    /// </summary>
    /// <returns>A list of IAvroAvscTypeHandlers.</returns>
    public IEnumerable<IAvroAvscTypeHandler> ResolveStrategies()
    {
        return _serviceProvider.GetServices<IAvroAvscTypeHandler>();
    }
}