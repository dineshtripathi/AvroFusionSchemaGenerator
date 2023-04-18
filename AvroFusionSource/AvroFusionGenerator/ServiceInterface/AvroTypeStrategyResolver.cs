using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.ServiceInterface;

public class AvroTypeStrategyResolver : IAvroTypeStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public AvroTypeStrategyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<IAvroAvscTypeHandler> ResolveStrategies()
    {
        return _serviceProvider.GetServices<IAvroAvscTypeHandler>();
    }
}