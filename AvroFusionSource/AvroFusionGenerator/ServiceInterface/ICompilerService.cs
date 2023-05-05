namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The compiler service.
/// </summary>

public interface ICompilerService
{
    /// <summary>
    /// Loads the types from source.
    /// </summary>
    /// <param name="sourceDirectoryPath"></param>
    /// <param name="parentClassModelName"></param>
    /// <returns>A list of Types.</returns>
    List<Type> LoadTypesFromSource(string sourceDirectoryPath, string parentClassModelName);
}