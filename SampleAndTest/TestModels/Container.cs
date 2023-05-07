namespace Avro.SchemaGeneration.Sample.Model;

/// <summary>
/// 
/// </summary>
/// <param name="ContainerName"></param>
/// <param name="PartitionKey"></param>
public record Container(string? ContainerName, string? PartitionKey) : AzureResource;