using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;
/// <summary>
/// The spectre service provider type registrar.
/// </summary>

public class SpectreServiceProviderTypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection? _services;

    public SpectreServiceProviderTypeRegistrar(IServiceCollection? services)
    {
        _services = services;
    }

    /// <summary>
    /// Registers the.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="implementation">The implementation.</param>
    public void Register(Type service, Type implementation)
    {
        _services?.AddSingleton(service, implementation);
    }

    /// <summary>
    /// Registers the instance.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="implementation">The implementation.</param>
    public void RegisterInstance(Type service, object implementation)
    {
        _services?.AddSingleton(service, implementation);
    }

    /// <summary>
    /// Registers the lazy.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="factory">The factory.</param>
    public void RegisterLazy(Type service, Func<object> factory)
    {
        _services?.AddSingleton(service, factory);
    }

    /// <summary>
    /// Builds the.
    /// </summary>
    /// <returns>An ITypeResolver.</returns>
    public ITypeResolver Build()
    {
        var serviceProvider = _services?.BuildServiceProvider();
        return new SpectreServiceProviderTypeResolver(serviceProvider);
    }
}