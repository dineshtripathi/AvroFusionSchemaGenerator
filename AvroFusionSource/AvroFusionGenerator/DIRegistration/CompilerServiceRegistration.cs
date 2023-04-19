using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;
/// <summary>
/// The compiler service registration.
/// </summary>

public class CompilerServiceRegistration : ICompilerServiceRegistration
{
    /// <summary>
    /// Registers the compiler services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void RegisterCompilerServices(IServiceCollection? services)
    {
        services?.AddSingleton<ICompilerService, CompilerService>();
        services?.AddSingleton<IDynamicAssemblyGenerator, DynamicAssemblyGenerator>();
    }
}