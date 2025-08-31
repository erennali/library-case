using Library.Domain.Entities;
using ExportTemplate = Library.Domain.Entities.ExportTemplate;

namespace Library.Application.Abstractions.Repositories;

public interface IImportExportRepository
{
    // Import job methods
    Task<ImportJob> CreateImportJobAsync(ImportJob importJob, CancellationToken cancellationToken = default);
    Task<ImportJob> UpdateImportJobAsync(ImportJob importJob, CancellationToken cancellationToken = default);
    Task<ImportJob?> GetImportJobAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<ImportJob> Items, int TotalCount)> GetImportJobsAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default);

    // Export job methods
    Task<ExportJob> CreateExportJobAsync(ExportJob exportJob, CancellationToken cancellationToken = default);
    Task<ExportJob> UpdateExportJobAsync(ExportJob exportJob, CancellationToken cancellationToken = default);
    Task<ExportJob?> GetExportJobAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<ExportJob> Items, int TotalCount)> GetExportJobsAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default);

    // Template methods
    Task<IReadOnlyList<ExportTemplate>> GetExportTemplatesAsync(CancellationToken cancellationToken = default);
    Task<ExportTemplate?> GetExportTemplateAsync(int id, CancellationToken cancellationToken = default);
    Task<ExportTemplate> CreateExportTemplateAsync(ExportTemplate template, CancellationToken cancellationToken = default);
    Task<ExportTemplate> UpdateExportTemplateAsync(ExportTemplate template, CancellationToken cancellationToken = default);
    Task DeleteExportTemplateAsync(int id, CancellationToken cancellationToken = default);
}
