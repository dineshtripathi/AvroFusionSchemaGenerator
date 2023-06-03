namespace Avro.SchemaGeneration.Sample.Model;

public record EventHubSchema(string? SchemaName, string? SchemaVersion, string? SerializationType) : AzureResource;