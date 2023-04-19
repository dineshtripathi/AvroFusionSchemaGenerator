using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc primitive type handler.
/// </summary>

public class AvroAvscPrimitiveTypeHandler : IAvroAvscTypeHandler
{
    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) || type == typeof(TimeSpan) || type == typeof(Guid);
    }

    /// <summary>
    /// Thens the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        return MapCSharpTypeToAvroType(type);
    }

    /// <summary>
    /// Maps the c sharp type to avro type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A string.</returns>
    private string MapCSharpTypeToAvroType(Type type)
    {
        if (type == typeof(bool)) return "boolean";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(float)) return "float";
        if (type == typeof(double)) return "double";
        if (type == typeof(string)) return "string";
        if (type == typeof(byte[])) return "bytes";
        if (type == typeof(decimal)) return "fixed";
        if (type == typeof(DateTime) || type == typeof(DateTimeOffset)) return "long";
        if (type == typeof(TimeSpan)) return "long";
        if (type == typeof(Guid)) return "string";

        throw new NotSupportedException($"The type '{type.Name}' is not supported by the Avro schema generator.");
    }
}