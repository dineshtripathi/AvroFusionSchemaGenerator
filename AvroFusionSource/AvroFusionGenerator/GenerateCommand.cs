using System.Diagnostics;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;
/// <summary>
/// The generate command.
/// </summary>

public class GenerateCommand : AsyncCommand<GenerateCommand.Settings>
{
    private readonly IAvroSchemaGenerator _avroSchemaGenerator;
    private readonly ICompilerService _compilerService;


    public GenerateCommand(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
        _compilerService = compilerService;
    }

    /// <summary>
    /// Executes the async.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>A Task.</returns>
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var inputFile = settings.InputFile;
        var outputDir = settings.OutputDir;
        var @namespace = settings.Namespace;

        if (inputFile != null)
        {
            var sourceCode = await File.ReadAllTextAsync(inputFile);
            var types = _compilerService.LoadTypesFromSource(sourceCode);

            var parentType = GetMainParentType(types);
            var progressReporter = new ProgressReporter();
            if (parentType?.Name != null)
            {
                var schemaFromParentClassProperties =
                    _avroSchemaGenerator.GenerateAvroAvscSchema(types, parentType.Name, progressReporter);

                if (outputDir != null)
                {
                    var outputPath = Path.Combine(outputDir, $"{parentType.Namespace}.{parentType.Name}.avsc");
                    await File.WriteAllTextAsync(outputPath, schemaFromParentClassProperties);
                    RunAvroGen(outputPath);
                }
            }
        }

        AnsiConsole.MarkupLine("[green]Success![/]");

        return 0;
    }

    /// <summary>
    /// Gets the main parent type.
    /// </summary>
    /// <param name="types">The types.</param>
    /// <returns>A Type.</returns>
    public Type? GetMainParentType(IEnumerable<Type?> types)
    {
        var collection = types.ToList();
        var typeSet = new HashSet<Type?>(collection);
        return (from type in collection
            let properties = type.GetProperties()
            from property in properties
            where typeSet.Contains(property.PropertyType)
            select type).FirstOrDefault();
    }

    /// <summary>
    /// Runs the avro gen.
    /// </summary>
    /// <param name="schemaFilePath">The schema file path.</param>
    public static void RunAvroGen(string schemaFilePath)
    {
        if (!File.Exists(schemaFilePath))
        {
            Console.WriteLine($"The file '{schemaFilePath}' does not exist.");
            return;
        }

        var schemaFileDirectory = Path.GetDirectoryName(schemaFilePath);
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        process.StandardInput.WriteLine($"avrogen -s {schemaFilePath} {schemaFileDirectory}\\output --skip-directories");
        process.StandardInput.WriteLine("exit"); 
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine($"AvroGen output: {output}");
    }

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings : CommandSettings
    {
        [CommandOption("--input-file <INPUT_FILE>")]
        public string? InputFile { get; set; }

        [CommandOption("--output-dir <OUTPUT_DIR>")]
        public string? OutputDir { get; set; }

        [CommandOption("--namespace <NAMESPACE>")]
        public string? Namespace { get; set; }
    }
}