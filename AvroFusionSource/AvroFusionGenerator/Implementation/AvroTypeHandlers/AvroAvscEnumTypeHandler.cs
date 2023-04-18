using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscEnumTypeHandler : IAvroAvscTypeHandler
{
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsEnum;
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        var symbols = Enum.GetNames(type);
        return new Dictionary<string, object>
        {
            { "type", "enum" },
            { "name", type.Name },
            { "namespace", type.Namespace },
            { "symbols", symbols }
        };
    }
}