namespace Avro.SchemaGeneration.Sample.Model;

public struct ArmTemplateMetadata
{
    public string Sku { get; set; }
    public string PricingPlan { get; set; }
    public string Endpoints { get; set; }
    public List<string> IpAddressRange { get; set; }
}