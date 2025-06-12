using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Reports;

namespace EchoesOfUzbekistan.Application.Reports.UpdateReportStatus;

public record UpdateReportStatusCommand(Guid ReportId, ReportStatus status) : ICommand;
