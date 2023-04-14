using Autofac;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public class TypeStrategyRegistration : ITypeStrategyRegistration
{
    public void RegisterTypeStrategies(IServiceCollection? services, ContainerBuilder builder)
    {

        // Register types implementing IAvroTypeStrategy from the current assembly
        builder.RegisterAssemblyTypes(typeof(AvroFusionGenerator).Assembly)
            .Where(t => typeof(IAvroTypeStrategy).IsAssignableFrom(t))
            .AsImplementedInterfaces();
    }


}