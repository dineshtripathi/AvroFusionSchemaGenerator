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
    public static IServiceProvider RegisterAllServices(IServiceCollection? services)
    {
        ITypeStrategyRegistration typeStrategyRegistration = new TypeStrategyRegistration();
        typeStrategyRegistration.RegisterTypeStrategies(services);
        services?.AddSingleton<IAvroTypeStrategyResolver, AvroTypeStrategyResolver>();

        services.AddSingleton<IAvroSchemaGenerator, AvroSchemaGenerator>();
        services.AddSingleton<Lazy<IAvroSchemaGenerator>>(sp =>
            new Lazy<IAvroSchemaGenerator>(() => sp.GetRequiredService<IAvroSchemaGenerator>()));

        ICompilerServiceRegistration compilerServiceRegistration = new CompilerServiceRegistration();
        compilerServiceRegistration.RegisterCompilerServices(services);

        ICommandBuilderRegistration commandBuilderRegistration = new CommandBuilderRegistration();
        commandBuilderRegistration.RegisterCommandBuilder(services);

        services?.AddSingleton<ProgressReporter>();

        return services.BuildServiceProvider();
    }
}