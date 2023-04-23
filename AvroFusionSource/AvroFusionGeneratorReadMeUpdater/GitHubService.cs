using Octokit;
using System.Net.Http.Headers;

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

    public async Task<(string packageVersion, string packageName,string releaseNumber)> GetPackageVersionAndTagAsync()
    {
        var packageVersion =  GetEnvironmentVariableWithMessage("PACKAGE_VERSION", "PACKAGE_VERSION");
        var packageName = GetEnvironmentVariableWithMessage("PACKAGE_NAME", "GITHUB PACKAGE_NAME");
        var releaseNumber = GetEnvironmentVariableWithMessage("RELEASE_NUMBER", "GITHUB RELEASE_NUMBER"); 
        //var repoDetails = repository.Split('/');
        //var owner = repoDetails[0];
        //var repoName = repoDetails[1];
        //var repo = await _github.Repository.Get(owner, repoName);

        //if (refName != null && refName.StartsWith("refs/tags"))
        //{
        //    var release = await _github.Repository.Release.GetLatest(repo.Id);
        //    LogMessage($"RELEASE NAME :{release.Name} , RELEASE TAGNAME :{release.TagName}");
        //    return (release.Name, release.TagName);
        //}

        //var branch = await _github.Repository.Branch.Get(repo.Id, "main");
        //var commit = branch.Commit;
        return (packageVersion,packageName,releaseNumber);
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


