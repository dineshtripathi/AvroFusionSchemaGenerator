using System.CommandLine.Invocation;
using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public class CommandBuilderRegistration : ICommandBuilderRegistration
{
    public void RegisterCommandBuilder(IServiceCollection? services)
    {
        services?.AddSingleton<CommandBuilder, AvroFusionCommandBuilder>();
        services?.AddSingleton<ICommandHandler, GenerateCommandHandler>();
        services?.AddSingleton<GenerateCommandHandler>();
    }
}