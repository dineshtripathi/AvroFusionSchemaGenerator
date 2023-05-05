using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;

/// <summary>
/// The settings.
/// </summary>
/// <summary>
/// The settings.
/// </summary>
public  class SpectreConsoleSettings : CommandSettings
{
    [CommandOption("--source-directory-path")]
    [Required]
    public string DirectoryPath { get; set; }

    [CommandOption("--input-file")]
    [Required]
    public string InputFile { get; set; }

    [CommandOption("--output-dir")]
    [Required]
    public string OutputDir { get; set; }

    [CommandOption("--namespace")]
    public string? Namespace { get; set; }
}