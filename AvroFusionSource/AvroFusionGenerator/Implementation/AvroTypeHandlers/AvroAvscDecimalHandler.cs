using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscDecimalHandler : IAvroAvscTypeHandler
{
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.Name == "Decimal";
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        return "bytes";
    }
}