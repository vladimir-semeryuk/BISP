namespace EchoesOfUzbekistan.Application.Reports.GetInappropriateContentReports;
public class InappropriateContentReportBaseResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public Guid AudioGuideId { get; init; }
    public string AudioGuideTitle { get; set; }
    public string Reason { get; init; }
    public string Status { get; init; }
    public DateTime CreatedAt { get; init; }
}
