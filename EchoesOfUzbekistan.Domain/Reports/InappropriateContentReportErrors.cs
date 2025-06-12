using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Reports;
public static class InappropriateContentReportErrors
{
    public static readonly Error NotFound = new(
        "InappropriateContentReport.NotFound",
        "The specified report was not found.");
}