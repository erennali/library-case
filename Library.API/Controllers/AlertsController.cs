using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertsController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet("my")]
    public async Task<ActionResult<MemberAlertsDto>> GetMyAlerts(CancellationToken ct)
    {
        try
        {
            var alerts = await _alertService.GetMyAlertsAsync(ct);
            return Ok(alerts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<OverdueAlertsDto>> GetOverdueAlerts(CancellationToken ct)
    {
        try
        {
            var alerts = await _alertService.GetOverdueAlertsAsync(ct);
            return Ok(alerts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("fines")]
    public async Task<ActionResult<FineAlertsDto>> GetFineAlerts(CancellationToken ct)
    {
        try
        {
            var alerts = await _alertService.GetFineAlertsAsync(ct);
            return Ok(alerts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("reservations")]
    public async Task<ActionResult<ReservationAlertsDto>> GetReservationAlerts(CancellationToken ct)
    {
        try
        {
            var alerts = await _alertService.GetReservationAlertsAsync(ct);
            return Ok(alerts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("membership")]
    public async Task<ActionResult<MembershipAlertsDto>> GetMembershipAlerts(CancellationToken ct)
    {
        try
        {
            var alerts = await _alertService.GetMembershipAlertsAsync(ct);
            return Ok(alerts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<AlertsSummaryDto>> GetAlertsSummary(CancellationToken ct)
    {
        try
        {
            var summary = await _alertService.GetAlertsSummaryAsync(ct);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("dismiss/{alertId:int}")]
    public async Task<IActionResult> DismissAlert(int alertId, CancellationToken ct)
    {
        try
        {
            await _alertService.DismissAlertAsync(alertId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("dismiss-all")]
    public async Task<IActionResult> DismissAllAlerts(CancellationToken ct)
    {
        try
        {
            await _alertService.DismissAllAlertsAsync(ct);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("settings")]
    public async Task<ActionResult<AlertSettingsDto>> GetAlertSettings(CancellationToken ct)
    {
        try
        {
            var settings = await _alertService.GetAlertSettingsAsync(ct);
            return Ok(settings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("settings")]
    public async Task<ActionResult<AlertSettingsDto>> UpdateAlertSettings([FromBody] UpdateAlertSettingsRequest request, CancellationToken ct)
    {
        try
        {
            var settings = await _alertService.UpdateAlertSettingsAsync(request, ct);
            return Ok(settings);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}

