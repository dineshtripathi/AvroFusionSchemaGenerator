using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public interface ITypeStrategyRegistration
{
    public void RegisterTypeStrategies(IServiceCollection? services, ContainerBuilder builder);
}

