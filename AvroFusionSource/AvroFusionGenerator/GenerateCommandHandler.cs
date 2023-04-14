using System.CommandLine;
using System.CommandLine.Invocation;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Spectre.Console;

namespace AvroFusionGenerator;

public class GenerateCommandHandler : ICommandHandler
{
    private readonly IAvroSchemaGenerator _avroSchemaGenerator;
    private readonly ICompilerService _compilerService;

    public GenerateCommandHandler(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
        _compilerService = compilerService;
    }

    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        string? inputFile = context.ParseResult.GetValueForOption<string>(new Option<string>("--input-file", "Input file path."));
        string? outputDir = context.ParseResult.GetValueForOption<string>(new Option<string>("--output-dir", "Output directory path."));

        // Load types from the input file
        string sourceCode = await File.ReadAllTextAsync(inputFile);
        var types = _compilerService.LoadTypesFromSource(sourceCode);

        // Generate the combined Avro schema
        string mainClassName = "CombinedRecord";
        var progressReporter = new ProgressReporter(); // Create a progress reporter if needed
        string combinedSchema = _avroSchemaGenerator.GenerateCombinedSchema(types, mainClassName, progressReporter);

        // Save the combined Avro schema to the output directory
        string outputPath = Path.Combine(outputDir, $"{mainClassName}.avsc");
        await File.WriteAllTextAsync(outputPath, combinedSchema);

        AnsiConsole.MarkupLine("[green]Success![/]");

        return 0;
    }
}