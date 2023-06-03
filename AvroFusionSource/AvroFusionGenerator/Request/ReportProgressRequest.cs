using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator.Request;
/// <summary>
/// The report progress request.
/// </summary>

public class ReportProgressRequest : IRequest, MediatR.IRequest
{
    /// <summary>
    /// Gets or sets the progress.
    /// </summary>
    public int Progress { get; set; }
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string? Message { get; set; }
}