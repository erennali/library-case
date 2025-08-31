using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IDashboardService
{
    Task<DashboardOverviewDto> GetOverviewAsync(CancellationToken ct = default);
    Task<CirculationStatsDto> GetCirculationStatsAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<OverdueSummaryDto> GetOverdueSummaryAsync(CancellationToken ct = default);
    Task<FineSummaryDto> GetFineSummaryAsync(CancellationToken ct = default);
    Task<MemberActivitySummaryDto> GetMemberActivityAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<BookPopularitySummaryDto> GetBookPopularityAsync(int topCount, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<CategoryStatsDto> GetCategoryStatsAsync(CancellationToken ct = default);
    Task<LibrarianActivitySummaryDto> GetLibrarianActivityAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<PagedResult<RecentTransactionDto>> GetRecentTransactionsAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<RecentReservationDto>> GetRecentReservationsAsync(int page, int pageSize, CancellationToken ct = default);
    Task<SystemHealthDto> GetSystemHealthAsync(CancellationToken ct = default);
    Task<TrendsDto> GetTrendsAsync(string trendType, int days, string? groupBy, CancellationToken ct = default);
}

