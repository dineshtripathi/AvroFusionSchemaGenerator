using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscDateTimeHandler : IAvroAvscTypeHandler
{
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.Name == "DateTime";
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        return "long";
    }
}