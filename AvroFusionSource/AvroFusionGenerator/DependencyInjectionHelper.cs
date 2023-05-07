using AvroFusionGenerator.DIRegistration;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator;
/// <summary>
/// The dependency injection helper.
/// </summary>

public static class DependencyInjectionHelper
{
    /// <summary>
    /// Registers the all services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceProvider.</returns>
    public static IServiceProvider? RegisterAllServices(IServiceCollection? services)
    {
        ITypeStrategyRegistration typeStrategyRegistration = new TypeStrategyRegistration();
        if (services != null)
        {
            typeStrategyRegistration.RegisterTypeStrategies(services);
            services?.AddSingleton<IAvroTypeHandlerResolver, AvroTypeHandlerResolver>();
            services?.AddSingleton<IAvroFusionSchemaGenerator, AvroFusionSchemaGenerator>();
            services?.AddSingleton<Lazy<IAvroFusionSchemaGenerator>>(sp =>
                new Lazy<IAvroFusionSchemaGenerator>(sp.GetRequiredService<IAvroFusionSchemaGenerator>));

            ICompilerServiceRegistration compilerServiceRegistration = new CompilerServiceRegistration();
            compilerServiceRegistration.RegisterCompilerServices(services);

            ICommandBuilderRegistration commandBuilderRegistration = new CommandBuilderRegistration();
            commandBuilderRegistration.RegisterCommandBuilder(services);

            services?.AddSingleton<ProgressReporter>();

            if (services != null) return services.BuildServiceProvider();
        }

        return null;
    }
}