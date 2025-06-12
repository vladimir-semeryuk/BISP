using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Reports.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Reports.GetInappropriateContentReports;
internal class GetInappropriateContentReportsQueryHandler : IQueryHandler<GetInappropriateContentReportsQuery,PaginatedResponse<InappropriateContentReportBaseResponse>>
{
    private readonly IReportReadRepository _reportReadRepository;

    public GetInappropriateContentReportsQueryHandler(IReportReadRepository reportReadRepository)
    {
        _reportReadRepository = reportReadRepository;
    }

    public async Task<Result<PaginatedResponse<InappropriateContentReportBaseResponse>>> Handle(GetInappropriateContentReportsQuery request, CancellationToken cancellationToken)
    {
        var result = await _reportReadRepository.GetInappropriateContentReportsAsync(request.Filter);
        return result;
    }
}
