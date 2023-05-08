using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The avro type strategy resolver.
/// </summary>

public class AvroTypeHandlerResolver : IAvroTypeHandlerResolver
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroTypeHandlerResolver"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public AvroTypeHandlerResolver(IServiceProvider serviceProvider)
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