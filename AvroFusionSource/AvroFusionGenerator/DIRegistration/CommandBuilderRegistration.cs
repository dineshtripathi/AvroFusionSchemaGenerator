using System.CommandLine.Invocation;
using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using static AvroFusionGenerator.GenerateCommand;

namespace AvroFusionGenerator.DIRegistration;
/// <summary>
/// The command builder registration.
/// </summary>

public class CommandBuilderRegistration : ICommandBuilderRegistration
{
    /// <summary>
    /// Registers the command builder.
    /// </summary>
    /// <param name="services">The services.</param>
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