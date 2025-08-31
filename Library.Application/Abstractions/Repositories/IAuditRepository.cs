using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IAuditRepository
{
    Task<AuditLog?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetByUserAsync(int userId, string userType, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetByActionAsync(string action, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<AuditLog> CreateAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditLog>> GetRecentActivityAsync(int count, CancellationToken cancellationToken = default);
}

