namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro union type attribute.
/// </summary>

[AttributeUsage(AttributeTargets.Property)]
public class AvroUnionTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvroUnionTypeAttribute"/> class.
    /// </summary>
    /// <param name="unionTypes">The union types.</param>
    public AvroUnionTypeAttribute(params Type[] unionTypes)
    {
        UnionTypes = unionTypes;
    }

    public Type[] UnionTypes { get; }
}