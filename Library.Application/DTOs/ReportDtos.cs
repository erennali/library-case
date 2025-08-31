namespace Library.Application.DTOs;

public record GenerateReportRequest(
    string ReportType,
    string Format = "pdf",
    Dictionary<string, object>? Parameters = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
);

public record ReportRequest(
    string ReportType,
    string Format,
    Dictionary<string, object>? Parameters = null
);



public record CirculationSummaryReportDto(
    DateTime FromDate,
    DateTime ToDate,
    int TotalCheckouts,
    int TotalReturns,
    int TotalRenewals,
    int TotalOverdue,
    decimal TotalFines,
    double AverageBorrowDuration,
    List<DailyCirculationDto> DailyStats
);

public record ReportResponseDto(
    int Id,
    string ReportType,
    string Format,
    string Status,
    string? FilePath,
    string? FileUrl,
    long? FileSize,
    Dictionary<string, object>? Parameters,
    DateTime? GeneratedAt,
    DateTime? ScheduledAt,
    DateTime CreatedAt
);

public record ReportTypeDto(
    string Name,
    string DisplayName,
    string Description,
    List<string> SupportedFormats,
    List<string> RequiredParameters
);





public record ScheduleReportRequest(
    string ReportType,
    string CronExpression,
    string Format = "pdf",
    Dictionary<string, object>? Parameters = null,
    bool IsActive = true
);



public record ReportListDto(
    int Id,
    string ReportType,
    string Format,
    string Status,
    DateTime CreatedAt,
    DateTime? GeneratedAt
);



public record ScheduledReportDto(
    int Id,
    string ReportType,
    string Format,
    string CronExpression,
    bool IsActive,
    DateTime NextRun,
    DateTime CreatedAt
);

public record ReportParametersDto(
    string Name,
    string Type,
    string? Description = null,
    bool IsRequired = false,
    object? DefaultValue = null,
    List<string>? Options = null
);

public record ReportTemplateDto(
    string Name,
    string DisplayName,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive = true
);

public record ReportExecutionDto(
    int Id,
    string ReportType,
    string Format,
    string Status,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string? ErrorMessage = null,
    long? FileSize = null,
    string? FilePath = null
);

public record ReportExecutionRequest(
    string ReportType,
    string Format,
    Dictionary<string, object>? Parameters = null,
    bool ExecuteAsync = false
);

public record ReportExecutionResult(
    bool Success,
    string? FilePath = null,
    string? FileUrl = null,
    long? FileSize = null,
    string? ErrorMessage = null,
    DateTime? GeneratedAt = null
);

public record ReportScheduleDto(
    int Id,
    string ReportType,
    string Format,
    string CronExpression,
    bool IsActive,
    DateTime NextRun,
    DateTime CreatedAt,
    DateTime? LastRunAt = null
);

public record UpdateReportScheduleRequest(
    string CronExpression,
    bool IsActive,
    Dictionary<string, object>? Parameters = null
);

public record ReportHistoryDto(
    int Id,
    string ReportType,
    string Format,
    string Status,
    DateTime CreatedAt,
    DateTime? CompletedAt = null,
    long? FileSize = null,
    string? ErrorMessage = null
);

public record ReportHistoryRequest(
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? ReportType = null,
    string? Status = null,
    int Page = 1,
    int PageSize = 20
);

public record ReportExportRequest(
    string Format,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? ReportType = null,
    string? Status = null
);

public record ReportExportResult(
    bool Success,
    string? FilePath = null,
    string? FileUrl = null,
    long? FileSize = null,
    string? ErrorMessage = null
);

public record ReportTemplateRequest(
    string Name,
    string DisplayName,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive = true
);

public record UpdateReportTemplateRequest(
    string? DisplayName = null,
    string? Description = null,
    List<ReportParametersDto>? Parameters = null,
    List<string>? SupportedFormats = null,
    bool? IsActive = null
);

public record ReportTemplateResponseDto(
    int Id,
    string Name,
    string DisplayName,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ReportTemplateListDto(
    int Id,
    string Name,
    string DisplayName,
    string Description,
    bool IsActive,
    DateTime CreatedAt
);

public record ReportTemplateSearchRequest(
    string? Search = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 20
);

public record ReportTemplateSearchResult(
    List<ReportTemplateListDto> Templates,
    int TotalCount,
    int Page,
    int PageSize
);

public record ReportTemplateDeleteRequest(
    int Id,
    bool ForceDelete = false
);

public record ReportTemplateDeleteResult(
    bool Success,
    string? Message = null,
    int DeletedCount = 0
);

public record ReportTemplateCopyRequest(
    int SourceId,
    string NewName,
    string? NewDisplayName = null,
    string? NewDescription = null
);

public record ReportTemplateCopyResult(
    bool Success,
    int? NewTemplateId = null,
    string? Message = null
);

public record ReportTemplateImportRequest(
    string Name,
    string DisplayName,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive = true
);

public record ReportTemplateImportResult(
    bool Success,
    int? TemplateId = null,
    string? Message = null,
    List<string>? Warnings = null
);

public record ReportTemplateExportRequest(
    int TemplateId,
    string Format = "json"
);

public record ReportTemplateExportResult(
    bool Success,
    string? FilePath = null,
    string? FileUrl = null,
    long? FileSize = null,
    string? ErrorMessage = null
);

public record ReportTemplateValidationRequest(
    int TemplateId,
    Dictionary<string, object> Parameters
);

public record ReportTemplateValidationResult(
    bool IsValid,
    List<string> Errors,
    List<string> Warnings,
    Dictionary<string, object>? ValidatedParameters = null
);

public record ReportTemplateTestRequest(
    int TemplateId,
    Dictionary<string, object> Parameters,
    string Format = "pdf"
);

public record ReportTemplateTestResult(
    bool Success,
    string? FilePath = null,
    string? FileUrl = null,
    long? FileSize = null,
    string? ErrorMessage = null,
    TimeSpan ExecutionTime = default
);

public record ReportTemplatePermissionsDto(
    int TemplateId,
    List<string> AllowedRoles,
    List<string> AllowedUsers,
    bool IsPublic = false
);

public record UpdateReportTemplatePermissionsRequest(
    List<string>? AllowedRoles = null,
    List<string>? AllowedUsers = null,
    bool? IsPublic = null
);

public record ReportTemplateVersionDto(
    int Id,
    int TemplateId,
    string Version,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive,
    DateTime CreatedAt,
    string CreatedBy
);

public record CreateReportTemplateVersionRequest(
    int TemplateId,
    string Version,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive = true
);

public record ReportTemplateVersionListDto(
    int Id,
    int TemplateId,
    string Version,
    string Description,
    bool IsActive,
    DateTime CreatedAt,
    string CreatedBy
);

public record ReportTemplateVersionSearchRequest(
    int TemplateId,
    string? Version = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 20
);

public record ReportTemplateVersionSearchResult(
    List<ReportTemplateVersionListDto> Versions,
    int TotalCount,
    int Page,
    int PageSize
);

public record ReportTemplateVersionActivateRequest(
    int VersionId,
    bool IsActive = true
);

public record ReportTemplateVersionActivateResult(
    bool Success,
    string? Message = null
);

public record ReportTemplateVersionDeleteRequest(
    int VersionId,
    bool ForceDelete = false
);

public record ReportTemplateVersionDeleteResult(
    bool Success,
    string? Message = null,
    int DeletedCount = 0
);

public record ReportTemplateVersionCopyRequest(
    int SourceVersionId,
    string NewVersion,
    string? NewDescription = null
);

public record ReportTemplateVersionCopyResult(
    bool Success,
    int? NewVersionId = null,
    string? Message = null
);

public record ReportTemplateVersionImportRequest(
    int TemplateId,
    string Version,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive = true
);

public record ReportTemplateVersionImportResult(
    bool Success,
    int? VersionId = null,
    string? Message = null,
    List<string>? Warnings = null
);

public record ReportTemplateVersionExportRequest(
    int VersionId,
    string Format = "json"
);

public record ReportTemplateVersionExportResult(
    bool Success,
    string? FilePath = null,
    string? FileUrl = null,
    long? FileSize = null,
    string? ErrorMessage = null
);

public record ReportTemplateVersionValidationRequest(
    int VersionId,
    Dictionary<string, object> Parameters
);

public record ReportTemplateVersionValidationResult(
    bool IsValid,
    List<string> Errors,
    List<string> Warnings,
    Dictionary<string, object>? ValidatedParameters = null
);

public record ReportTemplateVersionTestRequest(
    int VersionId,
    Dictionary<string, object> Parameters,
    string Format = "pdf"
);

public record ReportTemplateVersionTestResult(
    bool Success,
    string? FilePath = null,
    string? FileUrl = null,
    long? FileSize = null,
    string? ErrorMessage = null,
    TimeSpan ExecutionTime = default
);

public record ReportTemplateVersionPermissionsDto(
    int VersionId,
    List<string> AllowedRoles,
    List<string> AllowedUsers,
    bool IsPublic = false
);

public record UpdateReportTemplateVersionPermissionsRequest(
    List<string>? AllowedRoles = null,
    List<string>? AllowedUsers = null,
    bool? IsPublic = null
);

public record ReportTemplateVersionHistoryDto(
    int Id,
    int TemplateId,
    string Version,
    string Description,
    List<ReportParametersDto> Parameters,
    List<string> SupportedFormats,
    bool IsActive,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt = null,
    string? UpdatedBy = null
);

public record ReportTemplateVersionHistoryRequest(
    int TemplateId,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? Version = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 20
);

public record ReportTemplateVersionHistoryResult(
    List<ReportTemplateVersionHistoryDto> Versions,
    int TotalCount,
    int Page,
    int PageSize
);

public record ReportTemplateVersionRollbackRequest(
    int VersionId,
    string? Reason = null
);

public record ReportTemplateVersionRollbackResult(
    bool Success,
    string? Message = null
);

public record ReportTemplateVersionCompareRequest(
    int Version1Id,
    int Version2Id
);

public record ReportTemplateVersionCompareResult(
    bool Success,
    Dictionary<string, object> Differences,
    List<string>? Warnings = null
);

public record ReportTemplateVersionMergeRequest(
    int SourceVersionId,
    int TargetVersionId,
    Dictionary<string, object> MergeOptions
);

public record ReportTemplateVersionMergeResult(
    bool Success,
    int? MergedVersionId = null,
    string? Message = null,
    List<string>? Conflicts = null
);

public record ReportTemplateVersionBranchRequest(
    int SourceVersionId,
    string NewVersion,
    string? NewDescription = null
);

public record ReportTemplateVersionBranchResult(
    bool Success,
    int? NewVersionId = null,
    string? Message = null
);

public record ReportTemplateVersionTagRequest(
    int VersionId,
    string Tag,
    string? Description = null
);

public record ReportTemplateVersionTagResult(
    bool Success,
    string? Message = null
);

public record ReportTemplateVersionUntagRequest(
    int VersionId,
    string Tag
);

public record ReportTemplateVersionUntagResult(
    bool Success,
    string? Message = null
);

public record ReportTemplateVersionTagsDto(
    int VersionId,
    List<string> Tags
);

public record ReportTemplateVersionTagsRequest(
    int VersionId
);

public record ReportTemplateVersionTagsResult(
    bool Success,
    List<string> Tags,
    string? Message = null
);

public record ReportTemplateVersionSearchByTagRequest(
    string Tag,
    int Page = 1,
    int PageSize = 20
);

public record ReportTemplateVersionSearchByTagResult(
    List<ReportTemplateVersionListDto> Versions,
    int TotalCount,
    int Page,
    int PageSize
);



public record OverdueSummaryReportDto(
    int TotalOverdueBooks,
    int TotalOverdueMembers,
    decimal TotalFines,
    List<OverdueItemDto> TopOverdueItems
);



public record MemberActivityReportDto(
    int MemberId,
    string MemberName,
    DateTime FromDate,
    DateTime ToDate,
    int TotalCheckouts,
    int TotalReturns,
    int TotalRenewals,
    int TotalOverdue,
    decimal TotalFines,
    List<ActivityItemDto> Activities
);

public record BookPopularityReportDto(
    DateTime? FromDate,
    DateTime? ToDate,
    List<PopularBookDto> PopularBooks
);





public record ActivityItemDto(
    DateTime Date,
    string Activity,
    string BookTitle,
    string? Notes
);

public record PopularBookDto(
    int BookId,
    string BookTitle,
    string Author,
    int CheckoutCount,
    int ReservationCount,
    double AverageRating
);

public record CirculationReportDto(
    DateTime FromDate,
    DateTime ToDate,
    int TotalCheckouts,
    int TotalReturns,
    int TotalRenewals,
    int TotalOverdue,
    decimal TotalFines,
    double AverageBorrowDuration,
    List<DailyCirculationDto> DailyStats
);






