using AvroFusionGenerator.Implementation;
using MediatR;

namespace AvroFusionGenerator.Request;
/// <summary>
/// The report progress request handler.
/// </summary>

public class ReportProgressRequestHandler : IRequestHandler<ReportProgressRequest>
{
    private readonly ProgressReporter _progressReporter;

    public ReportProgressRequestHandler(ProgressReporter progressReporter)
    {
        _progressReporter = progressReporter;
    }

    /// <summary>
    /// Handles the.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task.</returns>
    public Task Handle(ReportProgressRequest request, CancellationToken cancellationToken)
    {
        _progressReporter.UpdateProgress(request.Progress, request.Message);
        return Task.FromResult(Unit.Value);
    }
}