
namespace TestModels;

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