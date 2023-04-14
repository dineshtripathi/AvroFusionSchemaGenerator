using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public class CommandBuilderRegistration : ICommandBuilderRegistration
{
    public void RegisterCommandBuilder(IServiceCollection? services)
    {
        services?.AddSingleton<CommandBuilder, AvroFusionCommandBuilder>();
    }
}