using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;
using AvroFusionGenerator.Abstraction;

public class AvroFusionCommandBuilder : CommandBuilder
{
    public override RootCommand BuildRootCommand()
    {
        var rootCommand = new RootCommand();
        rootCommand.AddArgument(
            new Argument<string>("input-file")
            {
                Description = "Input file path.",
                Arity = ArgumentArity.ExactlyOne,
            });
        rootCommand.AddArgument(new Argument<string>("output-dir")
            {
                Description = "Output directory path.",
                Arity = ArgumentArity.ExactlyOne,
            });
        rootCommand.AddArgument(new Argument<string>("namespace")
        {
            Description = "The namespace of the generated Avro schemas.",
            Arity = ArgumentArity.ZeroOrOne,
        });
        // Add other arguments as needed
        
        return rootCommand;
    }
}