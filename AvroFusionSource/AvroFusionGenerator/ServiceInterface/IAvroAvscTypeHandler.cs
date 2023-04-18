namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroAvscTypeHandler
{
    bool IfCanHandleAvroAvscType(Type type);
    object ThenCreateAvroAvscType(Type type, HashSet<string> forAvroAvscGeneratedTypes);

}