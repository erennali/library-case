namespace Library.Application.DTOs;

public record GetAuditLogsRequest(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    string? Action = null,
    int? UserId = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
);

public record AuditLogResponseDto(
    int Id,
    string Action,
    string EntityType,
    int? EntityId,
    string? OldValues,
    string? NewValues,
    int? UserId,
    string UserType,
    string? UserEmail,
    string? IpAddress,
    string? UserAgent,
    DateTime Timestamp,
    string? AdditionalData
);

public record AuditLogSummaryDto(
    int TotalLogs,
    int TodayLogs,
    int ThisWeekLogs,
    int ThisMonthLogs,
    Dictionary<string, int> ActionsByType,
    Dictionary<string, int> UsersByType
);

public record EntityChangeDto(
    int Id,
    string EntityType,
    int EntityId,
    string Action,
    string? OldValues,
    string? NewValues,
    DateTime Timestamp,
    string? UserName
);

public record ExportAuditLogsRequest(
    string Format,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? Action = null,
    string? EntityType = null,
    int? UserId = null
);

public record PurgeAuditLogsRequest(
    DateTime BeforeDate,
    string? EntityType = null,
    string? Action = null,
    bool DryRun = false
);

public record PurgeAuditLogsResult(
    int LogsPurged,
    long SpaceFreed,
    List<string> Warnings,
    bool Success
);

public record AuditLogDto(
    int Id,
    string Action,
    string EntityType,
    int? EntityId,
    string? OldValues,
    string? NewValues,
    int? UserId,
    string UserType,
    string? UserEmail,
    string? IpAddress,
    string? UserAgent,
    DateTime Timestamp,
    string? AdditionalData
);

public record AuditSummaryDto(
    int TotalLogs,
    int TodayLogs,
    int ThisWeekLogs,
    int ThisMonthLogs,
    Dictionary<string, int> ActionsByType,
    Dictionary<string, int> UsersByType
);

public record EntityChangesDto(
    int Id,
    string EntityType,
    int EntityId,
    string Action,
    string? OldValues,
    string? NewValues,
    DateTime Timestamp,
    string? UserName
);

public record AuditRetentionPolicyDto(
    int RetentionDays,
    bool AutoPurge,
    List<string> ExcludedEntityTypes,
    List<string> ExcludedActions,
    DateTime LastPurgeDate,
    int LastPurgeCount
);

public record UpdateAuditRetentionPolicyRequest(
    int RetentionDays,
    bool AutoPurge,
    List<string> ExcludedEntityTypes,
    List<string> ExcludedActions
);
