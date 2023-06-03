namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroFusionFileReader
{
    /// <summary>
    /// Reads the all text.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A string.</returns>
    string ReadAllText(string path);
    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>An array of string.</returns>
    string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
}