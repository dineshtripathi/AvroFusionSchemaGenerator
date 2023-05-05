namespace TestModels;

/// <summary>
/// The cosmos d b.
/// </summary>
/// <param name="CosmosDbName"></param>
/// <param name="AccountType"></param>
/// <param name="APIType"></param>
/// <param name="Databases"></param>
public record CosmosDb(string? CosmosDbName, string? AccountType, string? APIType, List<Database>? Databases) : AzureResource;