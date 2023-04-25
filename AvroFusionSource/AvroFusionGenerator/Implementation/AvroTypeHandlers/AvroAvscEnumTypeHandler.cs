using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro avsc enum type handler.
/// </summary>

public class AvroAvscEnumTypeHandler : IAvroAvscTypeHandler
{
    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type? type)
    {
        return type is {IsEnum: true};
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        if (type != null)
        {
            var symbols = Enum.GetNames(type);
            return new Dictionary<string, object?>
            {
                {"type", "enum"},
                {"name", type.Name},
                {"namespace", type.Namespace},
                {"symbols", symbols}
            };
        }

        return null;
    }
}