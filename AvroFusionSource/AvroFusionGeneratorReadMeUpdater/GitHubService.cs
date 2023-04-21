using Octokit;
using System.Net.Http.Headers;

public class GitHubService : IGitHubService
{
    private readonly GitHubClient _github;

    public GitHubService(string token)
    {
        _github = new GitHubClient(new Octokit.ProductHeaderValue("UpdateReadme"))
        {
            Credentials = new Credentials(token)
        };
    }

    public async Task<(string packageVersion, string tag)> GetPackageVersionAndTagAsync()
    {
        string repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
        string refName = Environment.GetEnvironmentVariable("GITHUB_REF");

        var repoDetails = repository.Split('/');
        var owner = repoDetails[0];
        var repoName = repoDetails[1];
        var repo = await _github.Repository.Get(owner, repoName);

        if (refName.StartsWith("refs/tags"))
        {
            var release = await _github.Repository.Release.GetLatest( repo.Id);
            return (release.Name, release.TagName);
        }
        else
        {
            var branch = await _github.Repository.Branch.Get(repo.Id, "main");
            var commit = branch.Commit;
            return ("Development", commit.Sha.Substring(0, 7));
        }
    }
}