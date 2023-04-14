using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.Abstraction;

public abstract class AvroFusionGeneratorBase
{
    protected static IServiceProvider? FusionGeneratorServiceProvider;

    protected AvroFusionGeneratorBase(IServiceProvider? fusionGeneratorServiceProvider)
    {
      //  FusionGeneratorServiceProvider = fusionGeneratorServiceProvider;
    }

    protected static IServiceCollection? ConfigureServices()
    {
        var services = new ServiceCollection();
        DependencyInjectionHelper.RegisterAllServices(services);
        Services = services;
        FusionGeneratorServiceProvider = Services.BuildServiceProvider(); // Add this line
        return Services;
    }

    protected static T GetService<T>() where T : notnull
    {
        return (FusionGeneratorServiceProvider ?? throw new InvalidOperationException()).GetRequiredService<T>();
    }

    protected static IServiceCollection? Services { get; set; }
}
