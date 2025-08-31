using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IStatisticsService
{
    Task<StatisticsOverviewDto> GetOverviewAsync(CancellationToken ct = default);
    Task<CirculationStatisticsDto> GetCirculationStatisticsAsync(DateTime? fromDate, DateTime? toDate, string? period, CancellationToken ct = default);
    Task<BookStatisticsDto> GetBookStatisticsAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<MemberStatisticsDto> GetMemberStatisticsAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<FineStatisticsDto> GetFineStatisticsAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<CategoryStatisticsDto> GetCategoryStatisticsAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<LibrarianStatisticsDto> GetLibrarianStatisticsAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<TrendsStatisticsDto> GetTrendsAsync(string metric, int days, string? groupBy, CancellationToken ct = default);
    Task<ComparisonStatisticsDto> GetComparisonAsync(DateTime period1Start, DateTime period1End, DateTime period2Start, DateTime period2End, string metric, CancellationToken ct = default);
    Task<List<TopBookStatisticsDto>> GetTopBooksAsync(int topCount, DateTime? fromDate, DateTime? toDate, string? category, CancellationToken ct = default);
    Task<List<TopMemberStatisticsDto>> GetTopMembersAsync(int topCount, DateTime? fromDate, DateTime? toDate, string? membershipType, CancellationToken ct = default);
    Task<OverdueAnalysisDto> GetOverdueAnalysisAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<FineAnalysisDto> GetFineAnalysisAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<FileResultDto?> ExportStatisticsAsync(string format, DateTime? fromDate, DateTime? toDate, string? type, CancellationToken ct = default);
}

