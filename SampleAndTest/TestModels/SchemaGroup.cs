namespace TestModels;

/// <summary>
/// 
/// </summary>
public class SchemaGroup
{
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public List<Schema>? Schemas { get; set; }
}