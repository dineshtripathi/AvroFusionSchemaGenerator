namespace Avro.SchemaGeneration.Sample.Model;

/// <summary>
/// 
/// </summary>
/// <param name="DatabaseName"></param>
/// <param name="Containers"></param>
public record Database(string? DatabaseName, List<Container>? Containers) : AzureResource;