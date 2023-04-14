using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Implementation;

public class AvroPrimitiveTypeStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan) || type == typeof(Guid);
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        return MapCSharpTypeToAvroType(type);
    }

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