﻿using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;

/// <inheritdoc />
public class AvroFusionGeneratorEntryPoint : AvroFusionGeneratorBase
{
    private static IServiceCollection? _services;
    private static SpectreServiceProviderTypeRegistrar? _typeRegistrar;

    /// <summary>
    /// Mains the.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <returns>A Task.</returns>
    public static async Task<int> Main(string[] args)
    {
        var program = new AvroFusionGeneratorEntryPoint();
        return await program.Run(args);
    }

    /// <summary>
    /// Runs the.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <returns>A Task.</returns>
    public async Task<int> Run(string[] args)
    {
        _services = new ServiceCollection();
        _services = ConfigureServices(_services);

        _typeRegistrar = new SpectreServiceProviderTypeRegistrar(_services);

        var app = new CommandApp(_typeRegistrar);

        app.Configure(config =>
        {
            config.SetApplicationName("AvroFusionSchema");


            config.AddCommand<SpectreGenerateCommand>("generate").WithDescription("Generates Avro schema for C# models.")
                .WithExample(new[]
                    {"generate", "--source-directory-path", "C:\\ModelClassproject" ,"--input-file", "input.cs", "--output-dir", "output", "--namespace", "MyNamespace"});
            ;
        });

        return await app.RunAsync(args);
    }
}