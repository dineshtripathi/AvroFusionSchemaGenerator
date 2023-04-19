
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.Implementation.AvroTypeHandlers;

namespace TestModels
{
    public class ResourceGroup
    {
        public string ResourceGroupName { get; set; }
        public string ResourceGroupSubscriptionId { get; set; }
        public DateTime CreationDate { get; set; }
        public List<AppService> AppServices { get; set; }
        public Dictionary<string, string> ResourceGroupTags { get; set; }
        public Dictionary<string, StorageAccount> StorageAccounts { get; set; }
        public Dictionary<string, VirtualMachine> VirtualMachines { get; set; }
        public List<DataFactoryPipeline> DataFactoryPipelines { get; set; }
        [AvroUnionType(typeof(int), typeof(double))]
        public object DataFactoryPipelineCount { get; set; }
        public List<EventHub> EventHubs { get; set; }
        public List<EventGrid> EventGrids { get; set; }
        public List<SchemaGroup> SchemaGroups { get; set; }
        public List<Schema> Schemas { get; set; }
        public Dictionary<string, CosmosDB> CosmosDBs { get; set; }
    }
    public class AppService
    {
        public string AppServiceName { get; set; }
        public string Plan { get; set; }
        public string OperatingSystem { get; set; }
        public string RuntimeStack { get; set; }
        public AppServiceStatus Status { get; set; }
    }

    public class StorageAccount
    {
        public string StorageAccountName { get; set; }
        public string AccountType { get; set; }
        public string AccessTier { get; set; }
        public bool IsSecureTransferEnabled { get; set; }
    }

    public class VirtualMachine
    {
        public string VirtualMachineName { get; set; }
        public string Size { get; set; }
        public string OperatingSystem { get; set; }
        public VirtualMachineStatus Status { get; set; }
    }
    public enum AppServiceStatus
    {
        Running,
        Stopped,
        Restarting,
        Scaling
    }

    public enum VirtualMachineStatus
    {
        Running,
        Stopped,
        Deallocated,
        Deleting
    }
    public class DataFactoryPipeline
    {
        public string PipelineName { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class EventHub
    {
        public string EventHubName { get; set; }
        public int PartitionCount { get; set; }
        public TimeSpan MessageRetention { get; set; }
    }

    public class EventGrid
    {
        public string EventGridName { get; set; }
        public string TopicType { get; set; }
        public string InputSchema { get; set; }
    }

    public class SchemaGroup
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public List<Schema> Schemas { get; set; }
    }

    public class Schema
    {
        public string SchemaName { get; set; }
        public string SchemaVersion { get; set; }
        public string SerializationType { get; set; }
    }

    public class CosmosDB
    {
        public string CosmosDBName { get; set; }
        public string AccountType { get; set; }
        public string APIType { get; set; }
        public List<Database> Databases { get; set; }
    }

    public class Database
    {
        public string DatabaseName { get; set; }
        public List<Container> Containers { get; set; }
    }

    public class Container
    {
        public string ContainerName { get; set; }
        public string PartitionKey { get; set; }
    }

    public class AzureResource
    {
        public string Name { get; set; }
        public string ResourceType { get; set; }
        public string Location { get; set; }
        public string ResourceGroup { get; set; }
        [AvroDuplicateFieldAlias("AzureResourceSubscriptionId")]
        public string SubscriptionId { get; set; }
        public string ETag { get; set; }
        public IDictionary<string, string> AzureResourceTags { get; set; }
        [AvroUnionType(typeof(int), typeof(double))]
        public object ProvisioningState { get; set; }
    }

}
