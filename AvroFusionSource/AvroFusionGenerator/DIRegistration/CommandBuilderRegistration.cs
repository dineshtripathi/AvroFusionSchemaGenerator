using System.CommandLine.Invocation;
using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using static AvroFusionGenerator.GenerateCommand;

namespace AvroFusionGenerator.DIRegistration;

public class CommandBuilderRegistration : ICommandBuilderRegistration
{
    public void RegisterCommandBuilder(IServiceCollection? services)
    {
        services?.AddSingleton<CommandBuilder, AvroFusionCommandBuilder>();
        services?.AddSingleton<ICommandHandler, GenerateCommandHandler>();
        services?.AddSingleton<GenerateCommandHandler>();
        services?.AddSingleton<GenerateCommand>();
        services?.AddSingleton<SpectreServiceProviderTypeRegistrar>();
        services?.AddSingleton<SpectreServiceProviderTypeResolver>();
        services?.AddSingleton<DefaultCommand>();
        services?.AddSingleton<Settings>();
    }
}