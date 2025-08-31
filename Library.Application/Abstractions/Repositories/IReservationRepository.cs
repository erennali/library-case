using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Abstractions.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetByStatusAsync(ReservationStatus status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task<Reservation> UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CountActiveReservationsAsync(int bookId, CancellationToken cancellationToken = default);
    Task<bool> HasActiveReservationAsync(int memberId, int bookId, CancellationToken cancellationToken = default);
}
