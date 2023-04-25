using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc u int16 handler.
/// </summary>

public class AvroAvscUInt16Handler : IAvroAvscTypeHandler
{
    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type? type)
    {
        return type?.Name == "UInt16";
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        return "int";
    }
}