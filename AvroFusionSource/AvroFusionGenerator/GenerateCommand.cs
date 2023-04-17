using System.ComponentModel;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;

public class GenerateCommand : AsyncCommand<GenerateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--input-file <INPUT_FILE>")]
        public string InputFile { get; set; }

        [CommandOption("--output-dir <OUTPUT_DIR>")]
        public string OutputDir { get; set; }

        [CommandOption("--namespace <NAMESPACE>")]
        public string Namespace { get; set; }
    }

    private readonly IAvroSchemaGenerator _avroSchemaGenerator;
    private readonly ICompilerService _compilerService;

    public GenerateCommand(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
        _compilerService = compilerService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        string inputFile = settings.InputFile;
        string outputDir = settings.OutputDir;
        string @namespace = settings.Namespace;

        // Load types from the input file
        string sourceCode = await File.ReadAllTextAsync(inputFile);
        var types = _compilerService.LoadTypesFromSource(sourceCode);

        // Generate the combined Avro schema
        string parentClassModelName = types.First().Name;
        var progressReporter = new ProgressReporter(); // Create a progress reporter if needed
        string schemaFromParentClassProperties = _avroSchemaGenerator.GenerateCombinedSchema(types, parentClassModelName, progressReporter);

        // Save the combined Avro schema to the output directory
        string outputPath = Path.Combine(outputDir, $"{parentClassModelName}.avsc");
        await File.WriteAllTextAsync(outputPath, schemaFromParentClassProperties);

        AnsiConsole.MarkupLine("[green]Success![/]");

        return 0;
    }

    
}