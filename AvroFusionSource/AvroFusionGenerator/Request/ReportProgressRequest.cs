using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Request;
/// <summary>
/// The report progress request.
/// </summary>

public class ReportProgressRequest : IRequest, MediatR.IRequest
{
    public int Progress { get; set; }
    public string? Message { get; set; }
}