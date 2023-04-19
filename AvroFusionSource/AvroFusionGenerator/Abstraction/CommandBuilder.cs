using System.CommandLine;

namespace AvroFusionGenerator.Abstraction;
/// <summary>
/// The command builder.
/// </summary>

public abstract class CommandBuilder
{
    /// <summary>
    /// Builds the root command.
    /// </summary>
    /// <returns>A RootCommand.</returns>
    public abstract RootCommand BuildRootCommand();
}