using EchoesOfUzbekistan.Domain.Reports;

namespace EchoesOfUzbekistan.Application.Reports.GetInappropriateContentReports;
public record GetInappropriateContentReportsFilter(
    Guid? CreatedByUserId = null,
    int? PageNumber = 1,
    int? PageSize = 10,
    string? SearchQuery = null,
    ReportStatus? Status = null,
    ReportSortBy SortBy = ReportSortBy.CreatedAt,
    bool SortDescending = true
    );

public enum ReportSortBy
{
    CreatedAt = 0,
    UserId = 1,
    AudioGuideId = 2
}