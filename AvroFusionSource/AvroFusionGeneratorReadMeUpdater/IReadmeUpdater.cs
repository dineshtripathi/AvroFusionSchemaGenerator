public interface IReadmeUpdater
{
    // void GetPackage();
    /// <summary>
    /// Updates the readme file.
    /// </summary>
    /// <param name="packageUrl">The package url.</param>
    /// <param name="packageName">The package name.</param>
    /// <param name="packageVersion">The package version.</param>
    /// <param name="tag">The tag.</param>
    void UpdateReadmeFile(string packageUrl, string packageName, string packageVersion, string tag);
}