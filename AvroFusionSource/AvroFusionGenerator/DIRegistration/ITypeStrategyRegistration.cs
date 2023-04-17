using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public interface ITypeStrategyRegistration
{
    public void RegisterTypeStrategies(ContainerBuilder builder);

    public void RegisterTypeStrategies(IServiceCollection services);
}

