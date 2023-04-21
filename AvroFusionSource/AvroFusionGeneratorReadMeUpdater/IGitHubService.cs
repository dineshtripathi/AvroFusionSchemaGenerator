using System.Net;

public interface IGitHubService
{
    Task<(string packageVersion, string tag)> GetPackageVersionAndTagAsync();
}