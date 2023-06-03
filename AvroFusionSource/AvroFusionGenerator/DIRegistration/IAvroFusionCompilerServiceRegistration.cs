using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;
/// <summary>
/// The compiler service registration.
/// </summary>

public interface IAvroFusionCompilerServiceRegistration
{
    /// <summary>
    /// Registers the compiler services.
    /// </summary>
    /// <param name="services">The services.</param>
    void RegisterCompilerServices(IServiceCollection? services);
}