using System.Diagnostics;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Spectre.Console;
using Spectre.Console.Cli;
using static AvroFusionGenerator.SpectreGenerateCommand;

namespace AvroFusionGenerator;
/// <summary>
/// The generate command.
/// </summary>

public class SpectreGenerateCommand : AsyncCommand<SpectreConsoleSettings>
{
    private readonly IAvroFusionSchemaGenerator _avroFusionSchemaGenerator;
    private readonly IAvroFusionCompilerService _avroFusionCompilerService;
  
    public SpectreGenerateCommand(IAvroFusionSchemaGenerator avroFusionSchemaGenerator, IAvroFusionCompilerService avroFusionCompilerService)
    {
        _avroFusionSchemaGenerator = avroFusionSchemaGenerator;
        _avroFusionCompilerService = avroFusionCompilerService;
    }

    /// <summary>
    /// Executes the async.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>A Task.</returns>
    public override async Task<int> ExecuteAsync(CommandContext context, SpectreConsoleSettings settings)
    {
        var progressReporter = new ProgressReporter();
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var sourceDirectory = settings.DirectoryPath;
            var parentClassFilePathWithName = settings.InputFile;
            var outputDir = settings.OutputDir;
            var @namespace = settings.Namespace;

            {
                var parentClassModelName =  Path.GetFileNameWithoutExtension(parentClassFilePathWithName);
                var types = _avroFusionCompilerService.LoadDotNetTypesFromSource(sourceDirectory, parentClassModelName);

                var parentType = GetMainParentType(types, parentClassModelName);

                if (parentType?.Name != null)
                {
                    var schemaFromParentClassProperties =
                        _avroFusionSchemaGenerator.GenerateAvroFusionAvscSchema(types, parentType.Name, progressReporter);

                    {
                        var outputPath = Path.Combine(outputDir, $"{parentType.Namespace}.{parentType.Name}.avsc");
                        await File.WriteAllTextAsync(outputPath, schemaFromParentClassProperties);
                        RunAvroGen(outputPath);
                    }
                }
            }

            AnsiConsole.MarkupLine("[green]Success![/]");
        });

        return 0;
    }

    /// <summary>
    /// Gets the main parent type.
    /// </summary>
    /// <param name="types">The types.</param>
    /// <param name="modelclassName"></param>
    /// <returns>A Type.</returns>
    public Type? GetMainParentType(IEnumerable<Type?> types, string modelclassName)
    {
        var collection = types.ToList();
        var typeSet = new HashSet<Type?>(collection);
        return (from type in collection
            let properties = type.GetProperties()
            from property in properties
            where typeSet.Contains(property.PropertyType)
            select type).FirstOrDefault(t=>t?.Name == modelclassName);
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


    
}