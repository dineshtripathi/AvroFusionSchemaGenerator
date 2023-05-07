using System.Runtime.CompilerServices;
using AvroFusionGenerator.ServiceInterface;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace AvroFusionGenerator.Implementation;

internal class AvroFusionFileReader : IAvroFusionFileReader
{
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        return Directory.GetFiles(path, searchPattern, searchOption);
    }
}