using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroTupleTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    public AvroTupleTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    public bool IfCanHandleAvroAvscType(Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Tuple<>) || type.GetGenericTypeDefinition() == typeof(ValueTuple<>));
    }

    public object ThenCreateAvroAvscType(Type type, HashSet<string> generatedTypes)
    {
        var genericArguments = type.GetGenericArguments();
        var tupleElements = genericArguments.Select(arg => _avroSchemaGenerator.Value.GenerateAvroAvscType(arg, generatedTypes)).ToList();

        var elementType = new Dictionary<string, object>
        {
            { "type", "tuple" },
            { "name", $"{type.Name}_tuple" },
            { "namespace", type.Namespace },
            { "elements", tupleElements }
        };

        return elementType;
    }
}