using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<StatisticsOverviewDto>> GetOverview(CancellationToken ct)
    {
        try
        {
            var overview = await _statisticsService.GetOverviewAsync(ct);
            return Ok(overview);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("circulation")]
    public async Task<ActionResult<CirculationStatisticsDto>> GetCirculationStatistics(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? period = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _statisticsService.GetCirculationStatisticsAsync(fromDate, toDate, period, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("books")]
    public async Task<ActionResult<BookStatisticsDto>> GetBookStatistics(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _statisticsService.GetBookStatisticsAsync(fromDate, toDate, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("members")]
    public async Task<ActionResult<MemberStatisticsDto>> GetMemberStatistics(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _statisticsService.GetMemberStatisticsAsync(fromDate, toDate, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("fines")]
    public async Task<ActionResult<FineStatisticsDto>> GetFineStatistics(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _statisticsService.GetFineStatisticsAsync(fromDate, toDate, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("categories")]
    public async Task<ActionResult<CategoryStatisticsDto>> GetCategoryStatistics(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _statisticsService.GetCategoryStatisticsAsync(fromDate, toDate, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("librarians")]
    public async Task<ActionResult<LibrarianStatisticsDto>> GetLibrarianStatistics(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _statisticsService.GetLibrarianStatisticsAsync(fromDate, toDate, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("trends")]
    public async Task<ActionResult<TrendsStatisticsDto>> GetTrends(
        [FromQuery] string metric = "circulation",
        [FromQuery] int days = 30,
        [FromQuery] string? groupBy = null,
        CancellationToken ct = default)
    {
        try
        {
            var trends = await _statisticsService.GetTrendsAsync(metric, days, groupBy, ct);
            return Ok(trends);
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

    [HttpGet("comparison")]
    public async Task<ActionResult<ComparisonStatisticsDto>> GetComparison(
        [FromQuery] DateTime period1Start,
        [FromQuery] DateTime period1End,
        [FromQuery] DateTime period2Start,
        [FromQuery] DateTime period2End,
        [FromQuery] string metric = "circulation",
        CancellationToken ct = default)
    {
        try
        {
            var comparison = await _statisticsService.GetComparisonAsync(
                period1Start, period1End, period2Start, period2End, metric, ct);
            return Ok(comparison);
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

    [HttpGet("top-books")]
    public async Task<ActionResult<List<TopBookStatisticsDto>>> GetTopBooks(
        [FromQuery] int topCount = 10,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? category = null,
        CancellationToken ct = default)
    {
        try
        {
            var topBooks = await _statisticsService.GetTopBooksAsync(topCount, fromDate, toDate, category, ct);
            return Ok(topBooks);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("top-members")]
    public async Task<ActionResult<List<TopMemberStatisticsDto>>> GetTopMembers(
        [FromQuery] int topCount = 10,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? membershipType = null,
        CancellationToken ct = default)
    {
        try
        {
            var topMembers = await _statisticsService.GetTopMembersAsync(topCount, fromDate, toDate, membershipType, ct);
            return Ok(topMembers);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("overdue-analysis")]
    public async Task<ActionResult<OverdueAnalysisDto>> GetOverdueAnalysis(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var analysis = await _statisticsService.GetOverdueAnalysisAsync(fromDate, toDate, ct);
            return Ok(analysis);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("fine-analysis")]
    public async Task<ActionResult<FineAnalysisDto>> GetFineAnalysis(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var analysis = await _statisticsService.GetFineAnalysisAsync(fromDate, toDate, ct);
            return Ok(analysis);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportStatistics(
        [FromQuery] string format = "json",
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? type = null,
        CancellationToken ct = default)
    {
        try
        {
            var exportResult = await _statisticsService.ExportStatisticsAsync(format, fromDate, toDate, type, ct);

            if (exportResult == null)
                return NotFound();

            return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
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

