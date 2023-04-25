using System.Net;

public interface IGitHubService
{
    /// <summary>
    /// Gets the package version and tag async.
    /// </summary>
    /// <returns>A Task.</returns>
    Task<(string packageVersion, string packageName, string releaseNumber, string? packageUrl)> GetPackageVersionAndTagAsync();
}