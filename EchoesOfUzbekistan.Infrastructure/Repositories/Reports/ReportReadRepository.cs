using Dapper;
using EchoesOfUzbekistan.Application.Reports.GetInappropriateContentReports;
using EchoesOfUzbekistan.Application.Reports.Interfaces;
using System.Text;
using EchoesOfUzbekistan.Application.Abstractions.Data;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Reports;
internal class ReportReadRepository : IReportReadRepository
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public ReportReadRepository(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<PaginatedResponse<InappropriateContentReportBaseResponse>> GetInappropriateContentReportsAsync(GetInappropriateContentReportsFilter filter)
    {
        using var connection = _connectionFactory.GetDbConnection();

        var baseSql = @"
        FROM inappropriate_content_reports r
        INNER JOIN users u ON r.user_id = u.id
        INNER JOIN audio_guides ag ON r.audio_guide_id = ag.id
        WHERE 1=1
        ";

        var whereBuilder = new StringBuilder();
        var parameters = new DynamicParameters();

        if (filter.CreatedByUserId is not null)
        {
            whereBuilder.Append(" AND r.user_id = @CreatedByUserId");
            parameters.Add("CreatedByUserId", filter.CreatedByUserId);
        }

        if (filter.Status is not null)
        {
            whereBuilder.Append(" AND r.status = @Status");
            parameters.Add("Status", (int)filter.Status.Value);
        }

        string sortColumn = filter.SortBy switch
        {
            ReportSortBy.UserId => "u.id",
            ReportSortBy.AudioGuideId => "ag.id",
            _ => "r.created_at"
        };

        string sortDirection = filter.SortDescending ? "DESC" : "ASC";

        var selectSql = $@"
        SELECT 
            r.id AS Id,
            r.user_id AS UserId,
            u.email AS Email,
            r.audio_guide_id AS AudioGuideId,
            ag.title AS AudioGuideTitle,
            r.reason AS Reason,
            r.status AS Status,
            r.created_at AS CreatedAt
        {baseSql}
        {whereBuilder}
        ORDER BY {sortColumn} {sortDirection}
        LIMIT @Limit OFFSET @Offset";

        var countSql = $@"SELECT COUNT(*) {baseSql} {whereBuilder}";

        parameters.Add("Limit", filter.PageSize ?? 10);
        parameters.Add("Offset", ((filter.PageNumber ?? 1) - 1) * (filter.PageSize ?? 10));

        var data = await connection.QueryAsync<InappropriateContentReportBaseResponse>(selectSql, parameters);
        var count = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        return new PaginatedResponse<InappropriateContentReportBaseResponse>
        {
            Items = data.ToList(),
            TotalCount = count
        };
    }
}
