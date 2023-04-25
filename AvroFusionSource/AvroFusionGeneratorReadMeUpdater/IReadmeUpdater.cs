public interface IReadmeUpdater
{

    /// <summary>
    /// Updates the readme file.
    /// </summary>
    /// <param name="packageUrl">The package url.</param>
    /// <param name="packageName">The package name.</param>
    /// <param name="packageVersion">The package version.</param>
    /// <param name="releaseNumber">The release number.</param>
    /// <param name="releaseType">The package type.</param>
    void UpdateReadmeFile(string? packageUrl, string packageName, string packageVersion, string releaseNumber, string releaseType);

}