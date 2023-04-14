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
        string? inputFile = context.ParseResult.GetValueForArgument(new Argument<string>("input-file"));
        string? outputDir = context.ParseResult.RootCommandResult.GetValueForArgument(new Argument<string>("output-dir"));
        string? @namespace = context.ParseResult.RootCommandResult.GetValueForArgument(new Argument<string>("namespace"));


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