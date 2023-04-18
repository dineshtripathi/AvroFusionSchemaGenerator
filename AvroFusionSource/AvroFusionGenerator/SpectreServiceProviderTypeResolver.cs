using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;

public class SpectreServiceProviderTypeResolver : ITypeResolver
{
    private readonly IServiceProvider? _serviceProvider;

    public SpectreServiceProviderTypeResolver(IServiceProvider? serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object Resolve(Type? type)
    {
        return _serviceProvider?.GetRequiredService(type);
    }
}