using System.CommandLine;

namespace AvroFusionGenerator.Abstraction;

public class AvroFusionCommandBuilder : CommandBuilder
{
    public override RootCommand BuildRootCommand()
    {
        var rootCommand = new RootCommand
        {
            new Option<string>("--input-file", "Input file path.")
            {
                Description = "Input file path.",
                IsRequired = true,
            },
            new Option<string>("--output-dir", "Output directory path.")
            {
                Description = "Output directory path.",
                IsRequired = true,
            },
            new Argument<string>("namespace")
            {
                Description = "The namespace of the generated Avro schemas."
            }
            // Add other options as needed
        };

        return rootCommand;
    }
}