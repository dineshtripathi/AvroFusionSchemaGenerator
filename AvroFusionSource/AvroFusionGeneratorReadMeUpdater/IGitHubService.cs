using System.Net;

public interface IGitHubService
{
    Task<(string packageVersion, string packageName, string releaseNumber)> GetPackageVersionAndTagAsync();
    string GetPackageName();
}