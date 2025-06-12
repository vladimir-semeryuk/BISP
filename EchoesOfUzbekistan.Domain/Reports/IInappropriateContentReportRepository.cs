namespace EchoesOfUzbekistan.Domain.Reports;
public interface IInappropriateContentReportRepository
{
    Task<InappropriateContentReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(InappropriateContentReport report);
}
