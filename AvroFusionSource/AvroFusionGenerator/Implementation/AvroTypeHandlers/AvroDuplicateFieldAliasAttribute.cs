namespace AvroFusionGenerator.Implementation.AvroTypeHandlers;

public class AvroDuplicateFieldAliasAttribute : Attribute
{
    public string Alias { get; }

    public AvroDuplicateFieldAliasAttribute(string alias)
    {
        Alias = alias;
    }
}