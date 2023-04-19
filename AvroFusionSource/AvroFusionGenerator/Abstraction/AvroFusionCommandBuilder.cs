using System.CommandLine;

namespace AvroFusionGenerator.Abstraction;
/// <summary>
/// The avro fusion command builder.
/// </summary>

public class AvroFusionCommandBuilder : CommandBuilder
{
    /// <summary>
    /// Builds the root command.
    /// </summary>
    /// <returns>A RootCommand.</returns>
    public override RootCommand BuildRootCommand()
    {
        var rootCommand = new RootCommand();
        rootCommand.AddArgument(
            new Argument<string>("input-file")
            {
                Description = "Input file path.",
                Arity = ArgumentArity.ExactlyOne
            });
        rootCommand.AddArgument(new Argument<string>("output-dir")
        {
            Description = "Output directory path.",
            Arity = ArgumentArity.ExactlyOne
        });
        rootCommand.AddArgument(new Argument<string>("namespace")
        {
            Description = "The namespace of the generated Avro schemas.",
            Arity = ArgumentArity.ZeroOrOne
        });
        return rootCommand;
    }
}