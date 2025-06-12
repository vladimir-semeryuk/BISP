using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Reports;

namespace EchoesOfUzbekistan.Application.Reports.UpdateReportStatus;
internal class UpdateReportStatusCommandHandler : ICommandHandler<UpdateReportStatusCommand>
{
    private readonly IInappropriateContentReportRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReportStatusCommandHandler(IUnitOfWork unitOfWork, IInappropriateContentReportRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<Result> Handle(UpdateReportStatusCommand request, CancellationToken cancellationToken)
    {
        var report = await _repository.GetByIdAsync(request.ReportId, cancellationToken);
        if (report == null)
            return Result.Failure(InappropriateContentReportErrors.NotFound);

        switch (request.status)
        {   
            case ReportStatus.Resolved:
                report.MarkAsResolved();
                break;
            case ReportStatus.Dismissed:
                report.MarkAsDismissed();
                break;
            default:
                break;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
