using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateAsync(CreateReservationRequest request, CancellationToken ct = default);
    Task CancelAsync(CancelReservationRequest request, CancellationToken ct = default);
    Task<ReservationResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<ReservationResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<ReservationResponseDto>> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<ReservationResponseDto>> GetActiveAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<ReservationResponseDto>> GetExpiredAsync(int page, int pageSize, CancellationToken ct = default);
    Task FulfillAsync(int id, CancellationToken ct = default);
}

