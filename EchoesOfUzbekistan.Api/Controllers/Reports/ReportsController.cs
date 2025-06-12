using EchoesOfUzbekistan.Api.Controllers.Users;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Reports.CreateInappropriateContentReport;
using EchoesOfUzbekistan.Application.Reports.GetInappropriateContentReports;
using EchoesOfUzbekistan.Application.Reports.UpdateReportStatus;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Reports;
[Route("api/[controller]")]
[ApiController]
public class ReportsController : AppControllerBase
{
    public ReportsController(ISender sender, IUserContextService userContextService) : base(sender, userContextService)
    {
    }

    [Authorize(Roles = $"{Roles.Administrator},{Roles.Moderator}")]
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<InappropriateContentReportBaseResponse>>> GetReports(
        [FromQuery] GetInappropriateContentReportsFilter filter)
    {
        var result = await _sender.Send(new GetInappropriateContentReportsQuery(filter));
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ReportInappropriateContent([FromBody] CreateInappropriateContentReportCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [Authorize(Roles = $"{Roles.Administrator},{Roles.Moderator}")]
    [HttpPut("{id}/resolve")]
    public async Task<IActionResult> ResolveReport([FromRoute] Guid id)
    {
        var result = await _sender.Send(new UpdateReportStatusCommand(id, ReportStatus.Resolved));
        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
    
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Moderator}")]
    [HttpPut("{id}/dismiss")]
    public async Task<IActionResult> DismissReport([FromRoute] Guid id)
    {
        var result = await _sender.Send(new UpdateReportStatusCommand(id, ReportStatus.Dismissed));
        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}
