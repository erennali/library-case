using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Abstractions.Repositories;

public interface IFineRepository
{
    Task<Fine?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetByStatusAsync(FineStatus status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Fine> CreateAsync(Fine fine, CancellationToken cancellationToken = default);
    Task<Fine> UpdateAsync(Fine fine, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalFinesByMemberAsync(int memberId, CancellationToken cancellationToken = default);
    Task<int> CountUnpaidFinesAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetPendingAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetOverdueAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
