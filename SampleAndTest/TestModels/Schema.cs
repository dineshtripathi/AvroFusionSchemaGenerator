namespace Avro.SchemaGeneration.Sample.Model;

public record Schema(string? SchemaName, string? SchemaVersion, string? SerializationType) : AzureResource;