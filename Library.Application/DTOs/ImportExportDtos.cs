namespace Library.Application.DTOs;

public record ImportRequest(
    string FileName,
    long FileSize,
    string ImportType,
    string Format,
    Dictionary<string, object>? Parameters = null
);

public record ImportResultDto(
    bool Success,
    int TotalRecords,
    int SuccessRecords,
    int FailedRecords,
    List<string> Errors,
    string? ResultFilePath = null
);

public record ExportRequest(
    string ExportType,
    string Format,
    Dictionary<string, object>? Filters = null,
    Dictionary<string, object>? Parameters = null
);

public record ExportResultDto(
    bool Success,
    string Status,
    string FileName,
    long FileSize,
    string DownloadUrl
);











public record ImportJobDto(
    int Id,
    string FileName,
    long FileSize,
    string ImportType,
    string Status,
    int TotalRecords,
    int ProcessedRecords,
    int SuccessRecords,
    int FailedRecords,
    List<string> Errors,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ExportJobDto(
    int Id,
    string ExportType,
    string Format,
    string Status,
    int TotalRecords,
    int ProcessedRecords,
    string FileName,
    long FileSize,
    string DownloadUrl,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ExportTemplateDto(
    int Id,
    string Name,
    string Description,
    string ExportType,
    string Format,
    List<string> Fields
);

public record ExportTemplate(
    int Id,
    string Name,
    string Description,
    string ExportType,
    string Format,
    List<string> Fields,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ImportExportStatsDto(
    int TotalImports,
    int TotalExports,
    int SuccessfulImports,
    int SuccessfulExports,
    int FailedImports,
    int FailedExports,
    Dictionary<string, List<string>> TypeFormats
);

public record ValidateDataRequest(
    string Data,
    string Format,
    string ImportType
);

public record ValidationResultDto(
    bool IsValid,
    List<string> Errors,
    List<string> Warnings,
    int ValidRecords,
    int InvalidRecords
);

public record ScheduleExportRequest(
    string ExportType,
    string Format,
    string CronExpression,
    Dictionary<string, object>? Filters = null,
    Dictionary<string, object>? Parameters = null
);

public record ScheduledExportDto(
    int Id,
    string ExportType,
    string Format,
    string CronExpression,
    bool IsActive,
    DateTime NextRun,
    DateTime CreatedAt
);

public record ImportHistoryDto(
    int Id,
    string FileName,
    string ImportType,
    string Status,
    int TotalRecords,
    int SuccessCount,
    int FailureCount,
    DateTime StartedAt,
    DateTime? CompletedAt
);

public record ExportHistoryDto(
    int Id,
    string ExportType,
    string Format,
    string Status,
    int TotalRecords,
    DateTime StartedAt,
    DateTime? CompletedAt
);

public record ImportDetailDto(
    int Id,
    string FileName,
    string ImportType,
    string Status,
    int TotalRecords,
    int SuccessCount,
    int FailureCount,
    int SkippedCount,
    List<string> Errors,
    List<string> Warnings,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string? ResultFilePath,
    Dictionary<string, object> Options
);

public record ExportDetailDto(
    int Id,
    string ExportType,
    string Format,
    string Status,
    int TotalRecords,
    string? FilePath,
    long? FileSize,
    Dictionary<string, object> Parameters,
    DateTime StartedAt,
    DateTime? CompletedAt
);

public record ValidationRuleDto(
    string Name,
    string Description,
    string RuleType,
    Dictionary<string, object> Parameters,
    bool IsActive
);

public record SupportedFormatsDto(
    List<string> ImportFormats,
    List<string> ExportFormats,
    Dictionary<string, List<string>> TypeFormats
);
