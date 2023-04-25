using AvroFusionGenerator.Implementation;

namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The avro schema generator.
/// </summary>

public interface IAvroSchemaGenerator
{
    /// <summary>
    /// Generates the avro avsc sshema.
    /// </summary>
    /// <param name="types">The types.</param>
    /// <param name="mainClassName">The main class name.</param>
    /// <param name="progressReporter">The progress reporter.</param>
    /// <returns>A string.</returns>
    string? GenerateAvroAvscSchema(IEnumerable<Type?> types, string mainClassName, ProgressReporter progressReporter);
    /// <summary>
    /// Generates the avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object.</returns>
    object? GenerateAvroAvscType(Type? type, HashSet<string> generatedTypes);
}