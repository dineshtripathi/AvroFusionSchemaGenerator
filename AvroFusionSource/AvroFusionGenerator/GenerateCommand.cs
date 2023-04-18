using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        var parentType = GetMainParentType(types);
        var progressReporter = new ProgressReporter(); // Create a progress reporter if needed
        string schemaFromParentClassProperties = _avroSchemaGenerator.GenerateAvroAvscSshema(types, parentType.Name, progressReporter);

        // Save the combined Avro schema to the output directory
        string outputPath = Path.Combine(outputDir, $"{parentType.Namespace}.{parentType.Name}.avsc");
        await File.WriteAllTextAsync(outputPath, schemaFromParentClassProperties);
        RunAvroGen(outputPath);
        AnsiConsole.MarkupLine("[green]Success![/]");

        return 0;
    }

    public Type GetMainParentType(IEnumerable<Type> types)
    {
        var typeSet = new HashSet<Type>(types);
        return (from type in types let properties = type.GetProperties() from property in properties where typeSet.Contains(property.PropertyType) select type).FirstOrDefault();
    }
    public static void RunAvroGen(string schemaFilePath)
    {
        if (!System.IO.File.Exists(schemaFilePath))
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
                CreateNoWindow = true,
            }
        };

        process.Start();

        // Execute the avrogen command
        process.StandardInput.WriteLine($"avrogen -s {schemaFilePath} {schemaFileDirectory}\\output --skip-directories");
        process.StandardInput.WriteLine("exit"); // Exit the command prompt
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine($"AvroGen output: {output}");
    }
}