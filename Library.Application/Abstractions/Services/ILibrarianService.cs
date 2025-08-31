using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface ILibrarianService
{
    Task<LibrarianResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<LibrarianResponseDto>> GetListAsync(int page, int pageSize, string? query, string? role, string? status, CancellationToken ct = default);
    Task<LibrarianResponseDto> CreateAsync(CreateLibrarianRequest request, CancellationToken ct = default);
    Task<LibrarianResponseDto> UpdateAsync(int id, UpdateLibrarianRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task ActivateAsync(int id, CancellationToken ct = default);
    Task DeactivateAsync(int id, CancellationToken ct = default);
    Task<LibrarianResponseDto> ChangeRoleAsync(int id, ChangeLibrarianRoleRequest request, CancellationToken ct = default);
    Task<LibrarianStatsDto> GetStatsAsync(CancellationToken ct = default);
    Task<PagedResult<LibrarianActivityDto>> GetActivityAsync(int id, int page, int pageSize, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
}

