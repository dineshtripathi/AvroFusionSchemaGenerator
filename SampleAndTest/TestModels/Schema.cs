namespace TestModels;

public record Schema(string? SchemaName, string? SchemaVersion, string? SerializationType) : AzureResource;