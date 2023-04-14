using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public interface ICommandBuilderRegistration
{
    public void RegisterCommandBuilder(IServiceCollection? services);
}