namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The compiler service.
/// </summary>

public interface ICompilerService
{
    /// <summary>
    /// Loads the types from source.
    /// </summary>
    /// <param name="sourceCode">The source code.</param>
    /// <returns>A list of Types.</returns>
    List<Type> LoadTypesFromSource(string sourceCode);
}