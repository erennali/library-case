using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IImportExportService
{
    Task<ImportResultDto> ImportAsync(ImportRequest request, CancellationToken ct = default);
    Task<FileResultDto?> ExportAsync(ExportRequest request, CancellationToken ct = default);
    Task<List<ExportTemplateDto>> GetExportTemplatesAsync(CancellationToken ct = default);
    Task<FileResultDto?> DownloadTemplateAsync(int id, CancellationToken ct = default);
    Task<PagedResult<ImportHistoryDto>> GetImportHistoryAsync(int page, int pageSize, string? status, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<PagedResult<ExportHistoryDto>> GetExportHistoryAsync(int page, int pageSize, string? type, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<ImportDetailDto?> GetImportDetailAsync(int id, CancellationToken ct = default);
    Task<ExportDetailDto?> GetExportDetailAsync(int id, CancellationToken ct = default);
    Task CancelImportAsync(int id, CancellationToken ct = default);
    Task<ImportResultDto> RetryImportAsync(int id, CancellationToken ct = default);
    Task<List<string>> GetSupportedFormatsAsync(CancellationToken ct = default);
    Task<List<ValidationRuleDto>> GetValidationRulesAsync(CancellationToken ct = default);
    Task<ValidationResultDto> ValidateDataAsync(ValidateDataRequest request, CancellationToken ct = default);
    Task<ScheduledExportDto> ScheduleExportAsync(ScheduleExportRequest request, CancellationToken ct = default);
    Task<List<ScheduledExportDto>> GetScheduledExportsAsync(CancellationToken ct = default);
    Task CancelScheduledExportAsync(int id, CancellationToken ct = default);
}

