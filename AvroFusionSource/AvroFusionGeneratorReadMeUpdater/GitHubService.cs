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

    public async Task<(string packageVersion, string packageName,string releaseNumber,string packageUrl)> GetPackageVersionAndTagAsync()
    {
       // var packageVersion =  GetEnvironmentVariableWithMessage("PACKAGE_VERSION", "PACKAGE_VERSION");
        var packageName = GetEnvironmentVariableWithMessage("PACKAGE_NAME", "GITHUB PACKAGE_NAME");
        //var releaseNumber = GetEnvironmentVariableWithMessage("RELEASE_NUMBER", "GITHUB RELEASE_NUMBER");
        var packageUrl = GetEnvironmentVariableWithMessage("PACKAGE_URL", "ARTIFACT PACKAGE URL");
        var pattern = @"(?<name>[^\.]+)\.(?<version>[0-9.]+)-beta(?<number>\d+)\.nupkg";
        Match match = Regex.Match(packageName, pattern);

        var packageVersion = match.Groups["version"].Value;
        var releaseNumber = match.Groups["number"].Value;
        packageName= match.Groups["name"].Value;

        return (packageVersion,packageName,releaseNumber,packageUrl);
    }

    public string GetPackageName()
    {
        var avroFusionGeneratorPackagePath = GetEnvironmentVariableWithMessage("NUGET_PACKAGE_PATH", "NUGET PACKAGE PATH");

        var files = Directory.GetFiles(avroFusionGeneratorPackagePath, "*.nupkg");
        LogMessage($"GITHUB NUGET_PACKAGE_PATH : {files}");

        var packageName = Path.GetFileName(files[0]);
        if (files.Length > 0)
        {
            return packageName;
        }

        throw new FileNotFoundException($"No .nupkg file found in the specified folder. {avroFusionGeneratorPackagePath}");
    }

    private string GetEnvironmentVariableWithMessage(string variable, string message)
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


