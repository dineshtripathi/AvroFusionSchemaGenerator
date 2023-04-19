using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;
/// <summary>
/// The type strategy registration.
/// </summary>

public interface ITypeStrategyRegistration
{
    /// <summary>
    /// Registers the type strategies.
    /// </summary>
    /// <param name="builder">The builder.</param>
    public void RegisterTypeStrategies(ContainerBuilder builder);

    /// <summary>
    /// Registers the type strategies.
    /// </summary>
    /// <param name="services">The services.</param>
    public void RegisterTypeStrategies(IServiceCollection services);
}