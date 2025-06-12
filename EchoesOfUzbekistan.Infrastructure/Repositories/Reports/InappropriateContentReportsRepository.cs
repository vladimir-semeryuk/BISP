using EchoesOfUzbekistan.Domain.Reports;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Reports;
internal class InappropriateContentReportRepository : Repository<InappropriateContentReport>, IInappropriateContentReportRepository
{
    public InappropriateContentReportRepository(AppDbContext context) : base(context)
    {
    }
}
