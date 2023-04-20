
using AvroFusionGenerator.Implementation.AvroTypeHandlers;

namespace TestModels
{
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
        public List<Schema>? Schemas { get; set; }
        public Dictionary<string, CosmosDb>? CosmosDBs { get; set; }
        public AzureResource? AzureResources { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppService
    {
        public string? AppServiceName { get; set; }
        public string? Plan { get; set; }
        public string? OperatingSystem { get; set; }
        public string? RuntimeStack { get; set; }
        public AppServiceStatus Status { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StorageAccount
    {
        public string? StorageAccountName { get; set; }
        public string? AccountType { get; set; }
        public string? AccessTier { get; set; }
        public bool IsSecureTransferEnabled { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class VirtualMachine
    {
        public string? VirtualMachineName { get; set; }
        public string? Size { get; set; }
        public string? OperatingSystem { get; set; }
        public VirtualMachineStatus Status { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum AppServiceStatus
    {
        Running,
        Stopped,
        Restarting,
        Scaling
    }
    /// <summary>
    /// 
    /// </summary>
    public enum VirtualMachineStatus
    {
        Running,
        Stopped,
        Deallocated,
        Deleting
    }
    /// <summary>
    /// 
    /// </summary>
    public class DataFactoryPipeline
    {
        public string? PipelineName { get; set; }
        public string? Description { get; set; }
        public DateTime LastModified { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EventHub
    {
        public string? EventHubName { get; set; }
        public int PartitionCount { get; set; }
        public TimeSpan MessageRetention { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EventGrid
    {
        public string? EventGridName { get; set; }
        public string? TopicType { get; set; }
        public string? InputSchema { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SchemaGroup
    {
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public List<Schema>? Schemas { get; set; }
    }

    public record Schema(string? SchemaName, string? SchemaVersion, string? SerializationType) : AzureResource;

    /// <summary>
    /// The cosmos d b.
    /// </summary>
    /// <param name="CosmosDbName"></param>
    /// <param name="AccountType"></param>
    /// <param name="APIType"></param>
    /// <param name="Databases"></param>
    public record CosmosDb(string? CosmosDbName, string? AccountType, string? APIType, List<Database>? Databases) : AzureResource;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DatabaseName"></param>
    /// <param name="Containers"></param>
    public record Database(string? DatabaseName, List<Container>? Containers) : AzureResource;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ContainerName"></param>
    /// <param name="PartitionKey"></param>
    public record Container(string? ContainerName, string? PartitionKey) : AzureResource;
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

}
