using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Reports;
public class InappropriateContentReport : Entity
{
    public Guid UserId { get; private set; }
    public Guid AudioGuideId { get; private set; }
    public string Reason { get; private set; }
    public ReportStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public InappropriateContentReport(Guid userId, Guid audioGuideId, string reason)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AudioGuideId = audioGuideId;
        Reason = reason;
        Status = ReportStatus.Pending;  // Default status is Pending
        CreatedAt = DateTime.UtcNow;
    }
    public void MarkAsResolved()
    {
        Status = ReportStatus.Resolved;
    }

    public void MarkAsDismissed()
    {
        Status = ReportStatus.Dismissed;
    }
}