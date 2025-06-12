using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Reports.CreateInappropriateContentReport;

public record CreateInappropriateContentReportCommand(Guid UserId, Guid AudioGuideId, string Reason) : ICommand<Guid>;
