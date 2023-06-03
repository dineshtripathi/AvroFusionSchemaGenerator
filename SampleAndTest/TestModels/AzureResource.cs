using AvroFusionGenerator.Implementation.AvroTypeHandlers;

namespace Avro.SchemaGeneration.Sample.Model;

/// <summary>
/// 
/// </summary>
public record AzureResource
{
    public string? Name { get; set; }
    public string? ResourceType { get; set; }
    public string? Location { get; set; }
    public string? ResourceGroup { get; set; }
    [AvroDuplicateFieldAlias("AzureResourceSubscriptionId")]
    public string? SubscriptionId { get; set; }
    public string? ETag { get; set; }
    public IDictionary<string, string>? AzureResourceTags { get; set; }
    [AvroUnionType(typeof(int), typeof(double))]
    public object? ProvisioningState { get; set; }
}