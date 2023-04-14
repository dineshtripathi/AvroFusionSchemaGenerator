using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public interface ICompilerServiceRegistration
{
    void RegisterCompilerServices(IServiceCollection? services);
}