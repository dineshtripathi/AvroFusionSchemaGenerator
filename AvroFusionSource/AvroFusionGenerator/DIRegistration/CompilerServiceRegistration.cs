using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public class CompilerServiceRegistration : ICompilerServiceRegistration
{
    public void RegisterCompilerServices(IServiceCollection? services)
    {
        services?.AddSingleton<ICompilerService, CompilerService>();
        services?.AddSingleton<IDynamicAssemblyGenerator, DynamicAssemblyGenerator>();
    }
}