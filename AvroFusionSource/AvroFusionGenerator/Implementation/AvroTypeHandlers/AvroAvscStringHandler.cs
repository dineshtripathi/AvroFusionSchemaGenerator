using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroAvscStringHandler : IAvroAvscTypeHandler
{
    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.Name == "String";
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        return "string";
    }
}