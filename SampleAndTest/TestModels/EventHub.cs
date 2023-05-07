namespace Avro.SchemaGeneration.Sample.Model;

/// <summary>
/// 
/// </summary>
public class EventHub
{
    public string? EventHubName { get; set; }
    public int PartitionCount { get; set; }
    public TimeSpan MessageRetention { get; set; }
}