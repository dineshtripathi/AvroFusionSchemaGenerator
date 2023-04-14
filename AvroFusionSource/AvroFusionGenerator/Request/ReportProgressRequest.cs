using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Request;

public class ReportProgressRequest : IRequest, MediatR.IRequest
{
    public int Progress { get; set; }
    public string? Message { get; set; }
}