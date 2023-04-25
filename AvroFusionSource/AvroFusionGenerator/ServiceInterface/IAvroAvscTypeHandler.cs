namespace AvroFusionGenerator.ServiceInterface;
/// <summary>
/// The avro avsc type handler.
/// </summary>

public interface IAvroAvscTypeHandler
{
    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    bool IfCanHandleAvroAvscType(Type? type);
    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes);
}