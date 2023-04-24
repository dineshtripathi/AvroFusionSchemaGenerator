using Octokit;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

public class GitHubService : IGitHubService
{
    private readonly GitHubClient _github;

    public GitHubService(string? token)
    {
        _github = new GitHubClient(new Octokit.ProductHeaderValue("UpdateReadme"))
        {
            Credentials = new Credentials(token)
        };
    }

    public Task<(string packageVersion, string packageName, string releaseNumber, string? packageUrl)> GetPackageVersionAndTagAsync()
    {
        var packageName = GetEnvironmentVariableWithMessage("PACKAGE_NAME", "GITHUB PACKAGE_NAME");
        var packageUrl = GetEnvironmentVariableWithMessage("PACKAGE_URL", "ARTIFACT PACKAGE URL");
        const string pattern = @"(?<name>[^\.]+)\.(?<version>[0-9.]+)-beta(?<number>\d+)\.nupkg";
        var match = Regex.Match(packageName, pattern);

        var packageVersion = match.Groups["version"].Value;
        var releaseNumber = match.Groups["number"].Value;
        packageName= match.Groups["name"].Value;

        return Task.FromResult((packageVersion,packageName,releaseNumber,packageUrl));
    }

    private static string? GetEnvironmentVariableWithMessage(string variable, string message)
    {
        var value = Environment.GetEnvironmentVariable(variable);
        Console.WriteLine($"{message} : {value}");
        return value;
    }

    private void LogMessage(string message)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine(message);
        Console.WriteLine("-----------------------------");
    }
}


