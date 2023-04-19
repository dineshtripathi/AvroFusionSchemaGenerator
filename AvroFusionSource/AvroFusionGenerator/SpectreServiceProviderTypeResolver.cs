using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;
/// <summary>
/// The spectre service provider type resolver.
/// </summary>

public class SpectreServiceProviderTypeResolver : ITypeResolver
{
    private readonly IServiceProvider? _serviceProvider;

    public SpectreServiceProviderTypeResolver(IServiceProvider? serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Resolves the.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>An object.</returns>
    public object Resolve(Type? type)
    {
        return _serviceProvider?.GetRequiredService(type);
    }
}