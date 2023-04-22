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

    public async Task<(string packageVersion, string tag)> GetPackageVersionAndTagAsync()
    {
        var repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");

        Console.WriteLine($"GitHub Repository :{repository}");
        var refName = Environment.GetEnvironmentVariable("GITHUB_REF");

        Console.WriteLine($"GITHUB REFERENCE : {refName}");
        
        var repoDetails = repository.Split('/');
        var owner = repoDetails[0];
        var repoName = repoDetails[1];
        var repo = await _github.Repository.Get(owner, repoName);

        if (refName != null && refName.StartsWith("refs/tags"))
        {
            var release = await _github.Repository.Release.GetLatest( repo.Id);
            return (release.Name, release.TagName);
        }

        var branch = await _github.Repository.Branch.Get(repo.Id, "main");
        var commit = branch.Commit;
        return ("Development", commit.Sha.Substring(0, 7));
    }

    public string GetPackageName()
    {
        var avroFusionGeneratorPackagePath = Environment.GetEnvironmentVariable("NUGET_PACKAGE_PATH");
        Console.WriteLine("-----------------------------");
        Console.WriteLine($"NUGET PACKAGE PATH : {avroFusionGeneratorPackagePath}");
        Console.WriteLine("-----------------------------");
        if (avroFusionGeneratorPackagePath != null)
        {
            var files = Directory.GetFiles(avroFusionGeneratorPackagePath, "*.nupkg");
            Console.WriteLine($"GITHUB NUGET_PACKAGE_PATH : {files}");
            Console.WriteLine("-----------------------------");
            var packageName = Path.GetFileName(files[0]);
            if (files.Length > 0)
            {
                return packageName;
            }
        }

        throw new FileNotFoundException($"No .nupkg file found in the specified folder. {avroFusionGeneratorPackagePath}");

    }
}