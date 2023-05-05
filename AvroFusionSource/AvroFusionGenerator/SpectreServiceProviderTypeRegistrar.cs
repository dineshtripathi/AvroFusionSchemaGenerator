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

    public void Register(Type service, Type implementation)
    {
        _services?.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _services?.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _services?.AddSingleton(service, _ => factory());
    }

    public void Register<TService, TImplementation>() where TService : class where TImplementation : class, TService
    {
        _services?.AddSingleton<TService, TImplementation>();
    }

    public void RegisterInstance<TService>(TService implementation) where TService : class
    {
        _services?.AddSingleton(implementation);
    }

    public void RegisterLazy<TService>(Func<TService> factory) where TService : class
    {
        _services?.AddSingleton(_ => factory());
    }

    public ITypeResolver Build()
    {
        var serviceProvider = _services?.BuildServiceProvider();
        return new SpectreServiceProviderTypeResolver(serviceProvider);
    }
}