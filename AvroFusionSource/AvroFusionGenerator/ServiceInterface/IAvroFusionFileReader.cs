namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroFusionFileReader
{
    string ReadAllText(string path);
    string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
}