using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetByTypeAsync(string alertType, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetBySeverityAsync(string severity, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetActiveAlertsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Alert> CreateAsync(Alert alert, CancellationToken cancellationToken = default);
    Task<Alert> UpdateAsync(Alert alert, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CountActiveAlertsAsync(CancellationToken cancellationToken = default);
}

