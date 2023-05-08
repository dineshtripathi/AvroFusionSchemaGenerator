using AvroFusionGenerator.Implementation.AvroTypeHandlers;

namespace Avro.SchemaGeneration.Sample.Model;

/// <summary>
/// 
/// </summary>
public class ResourceGroup
{
    public string? ResourceGroupName { get; set; }
    public string? ResourceGroupSubscriptionId { get; set; }
    public DateTime CreationDate { get; set; }
    public List<AppService>? AppServices { get; set; }
    public Dictionary<string, string>? ResourceGroupTags { get; set; }
    public Dictionary<string, StorageAccount>? StorageAccounts { get; set; }
    public Dictionary<string, VirtualMachine>? VirtualMachines { get; set; }
    public List<DataFactoryPipeline>? DataFactoryPipelines { get; set; }
    [AvroUnionType(typeof(int), typeof(double))]
    public object? DataFactoryPipelineCount { get; set; }
    public List<EventHub>? EventHubs { get; set; }
    public List<EventGrid>? EventGrids { get; set; }
    public List<SchemaGroup>? SchemaGroups { get; set; }
    public List<EventHubSchema>? Schemas { get; set; }
    public Dictionary<string, CosmosDb>? CosmosDBs { get; set; }
    public AzureResource? AzureResources { get; set; }
    public ArmTemplateMetadata? ArmTemplateMetadata { get; set; }
   // public Tuple<string,string,int,List<string>>? MappingResources { get; set; }
}