using AvroFusionGenerator.DIRegistration;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator;
/// <summary>
/// The dependency injection helper.
/// </summary>

public static class AvroFusionDependencyInjectionHelper
{
    /// <summary>
    /// Registers the all services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceProvider.</returns>
    public static IServiceProvider? RegisterAllServices(IServiceCollection? services)
    {
        IAvroFusionTypeStrategyRegistration avroFusionTypeStrategyRegistration = new AvroFusionTypeStrategyRegistration();
        if (services != null)
        {
            avroFusionTypeStrategyRegistration.RegisterTypeStrategies(services);
            services?.AddSingleton<IAvroTypeHandlerResolver, AvroTypeHandlerResolver>();
            services?.AddSingleton<IAvroFusionSchemaGenerator, AvroFusionSchemaGenerator>();
            services?.AddSingleton<Lazy<IAvroFusionSchemaGenerator>>(sp =>
                new Lazy<IAvroFusionSchemaGenerator>(sp.GetRequiredService<IAvroFusionSchemaGenerator>));

            IAvroFusionCompilerServiceRegistration avroFusionCompilerServiceRegistration = new AvroFusionCompilerServiceRegistration();
            avroFusionCompilerServiceRegistration.RegisterCompilerServices(services);

            IAvroFusionCommandBuilderRegistration avroFusionCommandBuilderRegistration = new AvroFusionCommandBuilderRegistration();
            avroFusionCommandBuilderRegistration.RegisterCommandBuilder(services);

            services?.AddSingleton<ProgressReporter>();

            if (services != null) return services.BuildServiceProvider();
        }

        return null;
    }
}