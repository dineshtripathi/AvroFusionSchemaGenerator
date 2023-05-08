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
    /// <summary>
    /// Gets or sets the directory path.
    /// </summary>
    [CommandOption("--source-directory-path")]
    [Required]
    public string DirectoryPath{ get; set; }

    /// <summary>
    /// Gets or sets the input file.
    /// </summary>
    [CommandOption("--input-file")]
    [Required]
    public string InputFile { get; set; }

    /// <summary>
    /// Gets or sets the output dir.
    /// </summary>
    [CommandOption("--output-dir")]
    [Required]
    public string OutputDir { get; set; }

    /// <summary>
    /// Gets or sets the namespace.
    /// </summary>
    [CommandOption("--namespace")]
    public string? Namespace { get; set; }
}