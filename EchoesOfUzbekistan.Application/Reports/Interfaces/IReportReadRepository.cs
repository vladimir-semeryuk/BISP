using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Reports.GetInappropriateContentReports;

namespace EchoesOfUzbekistan.Application.Reports.Interfaces;
public interface IReportReadRepository
{
    Task<PaginatedResponse<InappropriateContentReportBaseResponse>> GetInappropriateContentReportsAsync(
        GetInappropriateContentReportsFilter filter);
}
