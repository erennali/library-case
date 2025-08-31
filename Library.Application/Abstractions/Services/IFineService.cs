using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IFineService
{
    Task<FineResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<FineResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<FineResponseDto>> GetPendingAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<FineResponseDto>> GetOverdueAsync(int page, int pageSize, CancellationToken ct = default);
    Task<FineResponseDto> PayFineAsync(PayFineRequest request, CancellationToken ct = default);
    Task<FineResponseDto> WaiveFineAsync(WaiveFineRequest request, CancellationToken ct = default);
    Task<FineSummaryDto> GetMemberSummaryAsync(int memberId, CancellationToken ct = default);
    Task<OverallFineSummaryDto> GetOverallSummaryAsync(CancellationToken ct = default);
}

