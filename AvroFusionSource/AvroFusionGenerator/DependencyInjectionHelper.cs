using Autofac;
using AvroFusionGenerator.DIRegistration;
using AvroFusionGenerator.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator;

public static class DependencyInjectionHelper
{
    public static void RegisterAllServices(IServiceCollection? services)
    {
        var builder = new ContainerBuilder();
        ITypeStrategyRegistration typeStrategyRegistration = new TypeStrategyRegistration();
        typeStrategyRegistration.RegisterTypeStrategies(services,builder);

        ICompilerServiceRegistration compilerServiceRegistration = new CompilerServiceRegistration();
        compilerServiceRegistration.RegisterCompilerServices(services);

        ICommandBuilderRegistration commandBuilderRegistration = new CommandBuilderRegistration();
        commandBuilderRegistration.RegisterCommandBuilder(services);

        services?.AddSingleton<ProgressReporter>().AddSingleton<AvroSchemaGenerator>();
    }
}