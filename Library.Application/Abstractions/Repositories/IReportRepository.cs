using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IReportRepository
{
    Task<Report?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Report> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Report> Items, int TotalCount)> GetByTypeAsync(string reportType, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Report> Items, int TotalCount)> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Report> CreateAsync(Report report, CancellationToken cancellationToken = default);
    Task<Report> UpdateAsync(Report report, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Report>> GetRecentReportsAsync(int count, CancellationToken cancellationToken = default);
}

