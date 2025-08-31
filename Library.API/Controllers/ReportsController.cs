using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IValidator<GenerateReportRequest> _generateValidator;

    public ReportsController(
        IReportService reportService,
        IValidator<GenerateReportRequest> generateValidator)
    {
        _reportService = reportService;
        _generateValidator = generateValidator;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ReportResponseDto>> Generate([FromBody] GenerateReportRequest request, CancellationToken ct)
    {
        var validationResult = await _generateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _reportService.GenerateAsync(request, ct);
            return AcceptedAtAction(nameof(GetById), new { id = result.Id }, result);
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReportResponseDto>> GetById(int id, CancellationToken ct)
    {
        var report = await _reportService.GetByIdAsync(id, ct);
        if (report == null)
            return NotFound();

        return Ok(report);
    }

    [HttpGet("download/{id:int}")]
    public async Task<IActionResult> Download(int id, CancellationToken ct)
    {
        try
        {
            var fileResult = await _reportService.DownloadAsync(id, ct);
            if (fileResult == null)
                return NotFound();

            return File(fileResult.FileBytes, fileResult.ContentType, fileResult.FileName);
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

    [HttpGet("list")]
    public async Task<ActionResult<PagedResult<ReportResponseDto>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? reportType = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        var result = await _reportService.GetListAsync(page, pageSize, reportType, fromDate, toDate, ct);
        return Ok(result);
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<ReportTypeDto>>> GetAvailableTypes(CancellationToken ct)
    {
        var types = await _reportService.GetAvailableTypesAsync(ct);
        return Ok(types);
    }

    [HttpPost("schedule")]
    public async Task<ActionResult<ReportResponseDto>> Schedule([FromBody] ScheduleReportRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _reportService.ScheduleAsync(request, ct);
            return AcceptedAtAction(nameof(GetById), new { id = result.Id }, result);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _reportService.DeleteAsync(id, ct);
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

    [HttpGet("overdue-summary")]
    public async Task<ActionResult<OverdueSummaryReportDto>> GetOverdueSummary(CancellationToken ct)
    {
        try
        {
            var summary = await _reportService.GetOverdueSummaryAsync(ct);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("circulation-summary")]
    public async Task<ActionResult<CirculationSummaryReportDto>> GetCirculationSummary(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var summary = await _reportService.GetCirculationSummaryAsync(fromDate, toDate, ct);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("member-activity/{memberId:int}")]
    public async Task<ActionResult<MemberActivityReportDto>> GetMemberActivity(
        int memberId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var activity = await _reportService.GetMemberActivityAsync(memberId, fromDate, toDate, ct);
            return Ok(activity);
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

    [HttpGet("book-popularity")]
    public async Task<ActionResult<BookPopularityReportDto>> GetBookPopularity(
        [FromQuery] int topCount = 10,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var popularity = await _reportService.GetBookPopularityAsync(topCount, fromDate, toDate, ct);
            return Ok(popularity);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}

