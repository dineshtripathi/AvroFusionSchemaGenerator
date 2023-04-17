using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.Abstraction;

public abstract class AvroFusionGeneratorBase
{
    protected static IServiceProvider? FusionGeneratorServiceProvider;

    protected AvroFusionGeneratorBase()
    {
    }

    protected static IServiceCollection? ConfigureServices(IServiceCollection services)
    {
        FusionGeneratorServiceProvider=DependencyInjectionHelper.RegisterAllServices(services);
        return services;
    }

    protected static T GetService<T>() where T : notnull
    {
        return (FusionGeneratorServiceProvider ?? throw new InvalidOperationException()).GetRequiredService<T>();
    }

}
