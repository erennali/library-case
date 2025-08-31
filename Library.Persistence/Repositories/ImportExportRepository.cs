using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ExportTemplate = Library.Domain.Entities.ExportTemplate;

namespace Library.Persistence.Repositories;

public class ImportExportRepository : IImportExportRepository
{
    private readonly AppDbContext _context;

    public ImportExportRepository(AppDbContext context)
    {
        _context = context;
    }

    // Import job methods
    public async Task<ImportJob> CreateImportJobAsync(ImportJob importJob, CancellationToken cancellationToken = default)
    {
        _context.ImportJobs.Add(importJob);
        await _context.SaveChangesAsync(cancellationToken);
        return importJob;
    }

    public async Task<ImportJob> UpdateImportJobAsync(ImportJob importJob, CancellationToken cancellationToken = default)
    {
        _context.ImportJobs.Update(importJob);
        await _context.SaveChangesAsync(cancellationToken);
        return importJob;
    }

    public async Task<ImportJob?> GetImportJobAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ImportJobs
            .Include(j => j.CreatedByLibrarian)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<ImportJob> Items, int TotalCount)> GetImportJobsAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default)
    {
        var query = _context.ImportJobs
            .Include(j => j.CreatedByLibrarian)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(j => j.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    // Export job methods
    public async Task<ExportJob> CreateExportJobAsync(ExportJob exportJob, CancellationToken cancellationToken = default)
    {
        _context.ExportJobs.Add(exportJob);
        await _context.SaveChangesAsync(cancellationToken);
        return exportJob;
    }

    public async Task<ExportJob> UpdateExportJobAsync(ExportJob exportJob, CancellationToken cancellationToken = default)
    {
        _context.ExportJobs.Update(exportJob);
        await _context.SaveChangesAsync(cancellationToken);
        return exportJob;
    }

    public async Task<ExportJob?> GetExportJobAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ExportJobs
            .Include(j => j.CreatedByLibrarian)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<ExportJob> Items, int TotalCount)> GetExportJobsAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default)
    {
        var query = _context.ExportJobs
            .Include(j => j.CreatedByLibrarian)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(j => j.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    // Template methods
    public async Task<IReadOnlyList<ExportTemplate>> GetExportTemplatesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ExportTemplates
            .Where(t => !t.IsDefault) // Get non-default templates or adjust logic as needed
            .OrderBy(t => t.TemplateData)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExportTemplate?> GetExportTemplateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ExportTemplates
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<ExportTemplate> CreateExportTemplateAsync(ExportTemplate template, CancellationToken cancellationToken = default)
    {
        _context.ExportTemplates.Add(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task<ExportTemplate> UpdateExportTemplateAsync(ExportTemplate template, CancellationToken cancellationToken = default)
    {
        _context.ExportTemplates.Update(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task DeleteExportTemplateAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await _context.ExportTemplates.FindAsync(new object[] { id }, cancellationToken);
        if (template != null)
        {
            _context.ExportTemplates.Remove(template);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}



