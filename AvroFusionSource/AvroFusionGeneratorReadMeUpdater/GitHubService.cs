using System.Text.RegularExpressions;
using Octokit;

namespace AvroFusionGeneratorReadMeUpdater;

/// <summary>
/// The git hub service.
/// </summary>
public class GitHubService : IGitHubService
{
    private readonly GitHubClient _github;
    private const string Pattern = @"(?<name>[^\.]+)\.(?<version>[0-9.]+)-(?<releaseType>[^\d]+)(?<number>\d+)\.nupkg";
    public GitHubService(string? token)
    {
        _github = new GitHubClient(new Octokit.ProductHeaderValue("UpdateReadme"))
        {
            Credentials = new Credentials(token)
        };
    }

    /// <summary>
    /// Gets the package version and tag async.
    /// </summary>
    /// <returns>A Task.</returns>
    public Task<(string packageVersion, string packageName, string releaseNumber, string packageType, string? packageUrl)> GetPackageVersionAndTagAsync()
    {
        var packageName = GetEnvironmentVariableWithMessage("PACKAGE_NAME", "GITHUB PACKAGE_NAME");
        var packageUrl = GetEnvironmentVariableWithMessage("PACKAGE_URL", "ARTIFACT PACKAGE URL");

        if (packageName == null)
            return Task.FromResult((packageVersion: string.Empty, packageName: string.Empty,
                releaseNumber: string.Empty, packageType: string.Empty, packageUrl: (string?) null));

        var extractedProperties = Regex.Match(packageName, Pattern);

        var packageVersion = extractedProperties.Groups["version"].Value;
        var releaseNumber = extractedProperties.Groups["number"].Value;
        var packageType = extractedProperties.Groups["releaseType"].Value;
        packageName = extractedProperties.Groups["name"].Value;

        return Task.FromResult((packageVersion, packageName, releaseNumber, packageType, packageUrl));

    }


    /// <summary>
    /// Gets the environment variable with message.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <param name="message">The message.</param>
    /// <returns>A string? .</returns>
    private static string? GetEnvironmentVariableWithMessage(string variable, string message)
    {
        var value = Environment.GetEnvironmentVariable(variable);
        Console.WriteLine($"{message} : {value}");
        return value;
    }

    /// <summary>
    /// Logs the message.
    /// </summary>
    /// <param name="message">The message.</param>
    private void LogMessage(string message)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine(message);
        Console.WriteLine("-----------------------------");
    }
}