using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface ITransactionService
{
    Task<TransactionResponseDto> BorrowBookAsync(BorrowBookRequest request, CancellationToken ct = default);
    Task<TransactionResponseDto> ReturnBookAsync(ReturnBookRequest request, CancellationToken ct = default);
    Task<TransactionResponseDto> RenewBookAsync(RenewBookRequest request, CancellationToken ct = default);
    Task<TransactionResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<TransactionResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<TransactionResponseDto>> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<TransactionResponseDto>> GetOverdueAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<TransactionResponseDto>> GetActiveAsync(int page, int pageSize, CancellationToken ct = default);
}

