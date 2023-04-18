namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

[AttributeUsage(AttributeTargets.Property)]
public class AvroUnionTypeAttribute : Attribute
{
    public Type[] UnionTypes { get; }

    public AvroUnionTypeAttribute(params Type[] unionTypes)
    {
        UnionTypes = unionTypes;
    }
}