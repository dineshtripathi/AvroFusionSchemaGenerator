using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;

public class SpectreServiceProviderTypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _services;

    public SpectreServiceProviderTypeRegistrar(IServiceCollection services)
    {
        _services = services;
    }

    public void Register(Type service, Type implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _services.AddSingleton(service, factory);
    }

    public ITypeResolver Build()
    {
        var serviceProvider = _services.BuildServiceProvider();
        return new SpectreServiceProviderTypeResolver(serviceProvider);
    }
}