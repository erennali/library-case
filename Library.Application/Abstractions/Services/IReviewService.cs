using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IReviewService
{
    Task<ReviewResponseDto> CreateAsync(CreateReviewRequest request, CancellationToken ct = default);
    Task<ReviewResponseDto> UpdateAsync(int id, UpdateReviewRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<ReviewResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<ReviewResponseDto>> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<ReviewResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<ReviewResponseDto>> GetApprovedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<ReviewResponseDto>> GetPendingAsync(int page, int pageSize, CancellationToken ct = default);
    Task ApproveAsync(int id, CancellationToken ct = default);
    Task RejectAsync(int id, RejectReviewRequest request, CancellationToken ct = default);
    Task<BookReviewStatsDto> GetBookStatsAsync(int bookId, CancellationToken ct = default);
}

