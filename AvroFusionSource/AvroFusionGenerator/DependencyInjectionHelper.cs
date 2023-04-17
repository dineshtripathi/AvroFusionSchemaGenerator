using Autofac;
using Autofac.Extensions.DependencyInjection;
using AvroFusionGenerator.DIRegistration;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;
namespace AvroFusionGenerator;

public static class DependencyInjectionHelper
{
    public static IServiceProvider RegisterAllServices(IServiceCollection? services)
    {
        var builder = new ContainerBuilder();
        ITypeStrategyRegistration typeStrategyRegistration = new TypeStrategyRegistration();
       // typeStrategyRegistration.RegisterTypeStrategies(builder);
         typeStrategyRegistration.RegisterTypeStrategies(services);
         services?.AddSingleton<IAvroTypeStrategyResolver, AvroTypeStrategyResolver>();

         services.AddSingleton<IAvroSchemaGenerator, AvroSchemaGenerator>();
         services.AddSingleton<Lazy<IAvroSchemaGenerator>>(sp => new Lazy<IAvroSchemaGenerator>(() => sp.GetRequiredService<IAvroSchemaGenerator>()));


        ICompilerServiceRegistration compilerServiceRegistration = new CompilerServiceRegistration();
        compilerServiceRegistration.RegisterCompilerServices(services);

        ICommandBuilderRegistration commandBuilderRegistration = new CommandBuilderRegistration();
        commandBuilderRegistration.RegisterCommandBuilder(services);

        services?.AddSingleton<ProgressReporter>();
     

       // builder.Populate(services);
       // var container = builder.Build();

        // Use the Autofac container to create a service provider
       // var serviceProvider = new AutofacServiceProvider(container);
        return services.BuildServiceProvider();

    }
    
}