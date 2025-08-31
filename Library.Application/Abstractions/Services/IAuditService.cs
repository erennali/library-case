using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IAuditService
{
    Task<PagedResult<AuditLogDto>> GetAuditLogsAsync(int page, int pageSize, string? entityType, string? action, int? userId, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<AuditLogDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<AuditLogDto>> GetByEntityAsync(string entityType, int entityId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<AuditLogDto>> GetByUserAsync(int userId, int page, int pageSize, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<List<string>> GetAvailableActionsAsync(CancellationToken ct = default);
    Task<List<string>> GetAvailableEntityTypesAsync(CancellationToken ct = default);
    Task<AuditSummaryDto> GetSummaryAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<EntityChangesDto?> GetEntityChangesAsync(int id, CancellationToken ct = default);
    Task<FileResultDto?> ExportAuditLogsAsync(string format, DateTime? fromDate, DateTime? toDate, string? entityType, string? action, int? userId, CancellationToken ct = default);
    Task PurgeOldLogsAsync(PurgeAuditLogsRequest request, CancellationToken ct = default);
    Task<AuditRetentionPolicyDto> GetRetentionPolicyAsync(CancellationToken ct = default);
    Task<AuditRetentionPolicyDto> UpdateRetentionPolicyAsync(UpdateAuditRetentionPolicyRequest request, CancellationToken ct = default);
}

