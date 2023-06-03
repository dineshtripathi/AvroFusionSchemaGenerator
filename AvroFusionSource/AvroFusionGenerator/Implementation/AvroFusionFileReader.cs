using System.Runtime.CompilerServices;
using AvroFusionGenerator.ServiceInterface;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace AvroFusionGenerator.Implementation;

public class AvroFusionFileReader : IAvroFusionFileReader
{
    /// <summary>
    /// Reads the all text.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A string.</returns>
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>An array of string.</returns>
    public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        return Directory.GetFiles(path, searchPattern, searchOption);
    }
}