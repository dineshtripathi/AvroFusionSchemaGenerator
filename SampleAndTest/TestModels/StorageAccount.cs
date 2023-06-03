namespace Avro.SchemaGeneration.Sample.Model;

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