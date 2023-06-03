namespace Avro.SchemaGeneration.Sample.Model;

/// <summary>
/// 
/// </summary>
public class SchemaGroup
{
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public List<EventHubSchema>? Schemas { get; set; }
}