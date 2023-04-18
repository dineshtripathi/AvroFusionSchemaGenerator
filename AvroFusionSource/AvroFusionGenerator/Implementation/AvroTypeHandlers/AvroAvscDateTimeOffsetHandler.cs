using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscDateTimeOffsetHandler : IAvroAvscTypeHandler
{
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.Name == "DateTimeOffset";
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        return "long";
    }
}