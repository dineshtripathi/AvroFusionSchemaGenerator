using AvroFusionGenerator.Implementation;
using MediatR;

namespace AvroFusionGenerator.Request;

public class ReportProgressRequestHandler : IRequestHandler<ReportProgressRequest>
{
    private readonly ProgressReporter _progressReporter;

    public ReportProgressRequestHandler(ProgressReporter progressReporter)
    {
        _progressReporter = progressReporter;
    }

    public Task Handle(ReportProgressRequest request, CancellationToken cancellationToken)
    {
        _progressReporter.UpdateProgress(request.Progress, request.Message);
        return Task.FromResult(Unit.Value);
    }
}