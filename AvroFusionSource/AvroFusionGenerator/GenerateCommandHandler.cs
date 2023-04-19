﻿using System.CommandLine;
using System.CommandLine.Invocation;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Spectre.Console;

namespace AvroFusionGenerator;
/// <summary>
/// The generate command handler.
/// </summary>

public class GenerateCommandHandler : ICommandHandler
{
    private readonly IAvroSchemaGenerator _avroSchemaGenerator;
    private readonly ICompilerService _compilerService;

    public GenerateCommandHandler(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
        _compilerService = compilerService;
    }

    /// <summary>
    /// Invokes the.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>An int.</returns>
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Invokes the async.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task.</returns>
    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var inputFile = context.ParseResult.GetValueForArgument(new Argument<string>("input-file"));
        var outputDir = context.ParseResult.RootCommandResult.GetValueForArgument(new Argument<string>("output-dir"));
        var @namespace = context.ParseResult.RootCommandResult.GetValueForArgument(new Argument<string>("namespace"));


        // Load types from the input file
        var sourceCode = await File.ReadAllTextAsync(inputFile);
        var types = _compilerService.LoadTypesFromSource(sourceCode);

        // Generate the combined Avro schema
        var parentClassModelName = types.First().Name;
        var progressReporter = new ProgressReporter(); // Create a progress reporter if needed
        var schemaFromParentClassProperties =
            _avroSchemaGenerator.GenerateAvroAvscSshema(types, parentClassModelName, progressReporter);

        // Save the combined Avro schema to the output directory
        var outputPath = Path.Combine(outputDir, $"{parentClassModelName}.avsc");
        await File.WriteAllTextAsync(outputPath, schemaFromParentClassProperties);

        AnsiConsole.MarkupLine("[green]Success![/]");

        return 0;
    }
}