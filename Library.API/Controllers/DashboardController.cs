using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<DashboardOverviewDto>> GetOverview(CancellationToken ct)
    {
        try
        {
            var overview = await _dashboardService.GetOverviewAsync(ct);
            return Ok(overview);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("circulation-stats")]
    public async Task<ActionResult<CirculationStatsDto>> GetCirculationStats(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var stats = await _dashboardService.GetCirculationStatsAsync(fromDate, toDate, ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("overdue-summary")]
    public async Task<ActionResult<OverdueSummaryDto>> GetOverdueSummary(CancellationToken ct)
    {
        try
        {
            var summary = await _dashboardService.GetOverdueSummaryAsync(ct);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("fine-summary")]
    public async Task<ActionResult<FineSummaryDto>> GetFineSummary(CancellationToken ct)
    {
        try
        {
            var summary = await _dashboardService.GetFineSummaryAsync(ct);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("member-activity")]
    public async Task<ActionResult<MemberActivitySummaryDto>> GetMemberActivity(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var activity = await _dashboardService.GetMemberActivityAsync(fromDate, toDate, ct);
            return Ok(activity);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("book-popularity")]
    public async Task<ActionResult<BookPopularitySummaryDto>> GetBookPopularity(
        [FromQuery] int topCount = 10,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var popularity = await _dashboardService.GetBookPopularityAsync(topCount, fromDate, toDate, ct);
            return Ok(popularity);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("category-stats")]
    public async Task<ActionResult<CategoryStatsDto>> GetCategoryStats(CancellationToken ct)
    {
        try
        {
            var stats = await _dashboardService.GetCategoryStatsAsync(ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("librarian-activity")]
    public async Task<ActionResult<LibrarianActivitySummaryDto>> GetLibrarianActivity(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var activity = await _dashboardService.GetLibrarianActivityAsync(fromDate, toDate, ct);
            return Ok(activity);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("recent-transactions")]
    public async Task<ActionResult<PagedResult<RecentTransactionDto>>> GetRecentTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        try
        {
            var transactions = await _dashboardService.GetRecentTransactionsAsync(page, pageSize, ct);
            return Ok(transactions);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("recent-reservations")]
    public async Task<ActionResult<PagedResult<RecentReservationDto>>> GetRecentReservations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        try
        {
            var reservations = await _dashboardService.GetRecentReservationsAsync(page, pageSize, ct);
            return Ok(reservations);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("system-health")]
    public async Task<ActionResult<SystemHealthDto>> GetSystemHealth(CancellationToken ct)
    {
        try
        {
            var health = await _dashboardService.GetSystemHealthAsync(ct);
            return Ok(health);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("trends")]
    public async Task<ActionResult<TrendsDto>> GetTrends(
        [FromQuery] string trendType = "circulation",
        [FromQuery] int days = 30,
        CancellationToken ct = default)
    {
        try
        {
            var trends = await _dashboardService.GetTrendsAsync(trendType, days, null, ct);
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
}

