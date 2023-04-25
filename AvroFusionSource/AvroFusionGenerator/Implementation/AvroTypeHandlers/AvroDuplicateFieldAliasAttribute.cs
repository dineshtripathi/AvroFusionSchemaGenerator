namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;
/// <summary>
/// The avro duplicate field alias attribute.
/// </summary>

public class AvroDuplicateFieldAliasAttribute : Attribute
{
    public string Alias { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroDuplicateFieldAliasAttribute"/> class.
    /// </summary>
    /// <param name="alias">The alias.</param>
    public AvroDuplicateFieldAliasAttribute(string alias)
    {
        Alias = alias;
    }
}