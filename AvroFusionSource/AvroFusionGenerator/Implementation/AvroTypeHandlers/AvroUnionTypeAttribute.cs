namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro union type attribute.
/// </summary>

[AttributeUsage(AttributeTargets.Property)]
public class AvroUnionTypeAttribute : Attribute
{
    public AvroUnionTypeAttribute(params Type[] unionTypes)
    {
        UnionTypes = unionTypes;
    }

    public Type[] UnionTypes { get; }
}