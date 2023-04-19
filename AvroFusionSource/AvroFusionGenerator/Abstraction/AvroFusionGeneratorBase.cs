using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.Abstraction;
/// <summary>
/// The avro fusion generator base.
/// </summary>

public abstract class AvroFusionGeneratorBase
{
    protected static IServiceProvider? FusionGeneratorServiceProvider;

    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection? .</returns>
    protected static IServiceCollection? ConfigureServices(IServiceCollection? services)
    {
        FusionGeneratorServiceProvider = DependencyInjectionHelper.RegisterAllServices(services);
        return services;
    }

    /// <summary>
    /// Gets the service.
    /// </summary>
    /// <returns>A T.</returns>
    protected static T GetService<T>() where T : notnull
    {
        return (FusionGeneratorServiceProvider ?? throw new InvalidOperationException()).GetRequiredService<T>();
    }
}