namespace TestModels;

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