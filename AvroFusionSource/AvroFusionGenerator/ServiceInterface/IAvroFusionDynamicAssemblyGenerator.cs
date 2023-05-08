namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The avro fusion dynamic assembly generator.
/// </summary>

public interface IAvroFusionDynamicAssemblyGenerator
{
    /// <summary>
    ///     Generates the dynamic assembly.
    /// </summary>
    /// <param name="sourceDirectory"></param>
    /// <param name="csharpModelParentClass">The csharp model parent class.</param>
    /// <returns>A list of Types.</returns>
    List<Type> GenerateDynamicAssembly(string sourceDirectory, string csharpModelParentClass);
}