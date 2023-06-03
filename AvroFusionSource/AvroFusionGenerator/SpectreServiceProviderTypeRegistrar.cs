using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;
/// <summary>
/// The spectre service provider type registrar.
/// </summary>

public class SpectreServiceProviderTypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection? _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreServiceProviderTypeRegistrar"/> class.
    /// </summary>
    /// <param name="services">The services.</param>
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
        _services?.AddSingleton(service, _ => factory());
    }

    /// <summary>
    /// Registers the.
    /// </summary>
    public void Register<TService, TImplementation>() where TService : class where TImplementation : class, TService
    {
        _services?.AddSingleton<TService, TImplementation>();
    }

    /// <summary>
    /// Registers the instance.
    /// </summary>
    /// <param name="implementation">The implementation.</param>
    public void RegisterInstance<TService>(TService implementation) where TService : class
    {
        _services?.AddSingleton(implementation);
    }

    /// <summary>
    /// Registers the lazy.
    /// </summary>
    /// <param name="factory">The factory.</param>
    public void RegisterLazy<TService>(Func<TService> factory) where TService : class
    {
        _services?.AddSingleton(_ => factory());
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