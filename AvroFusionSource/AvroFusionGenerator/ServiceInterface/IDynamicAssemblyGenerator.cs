namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The dynamic assembly generator.
/// </summary>

public interface IDynamicAssemblyGenerator
{
    /// <summary>
    /// Generates the dynamic assembly.
    /// </summary>
    /// <param name="sourceDirectory"></param>
    /// <param name="csharpModelParentClass">The csharp model parent class.</param>
    /// <returns>A list of Types.</returns>
    List<Type> GenerateDynamicAssembly(string sourceDirectory, string csharpModelParentClass);
}