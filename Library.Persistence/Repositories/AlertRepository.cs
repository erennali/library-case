using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly AppDbContext _context;

    public AlertRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Alert?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .Include(a => a.AcknowledgedByLibrarian)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Alerts
            .Include(a => a.AcknowledgedByLibrarian)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a =>
                a.Title.Contains(search) ||
                a.Message.Contains(search) ||
                a.AlertType.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetByTypeAsync(string alertType, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Alerts
            .Include(a => a.AcknowledgedByLibrarian)
            .Where(a => a.AlertType == alertType);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetBySeverityAsync(string severity, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Alerts
            .Include(a => a.AcknowledgedByLibrarian)
            .Where(a => a.Severity == severity);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Alert> Items, int TotalCount)> GetActiveAlertsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Alerts
            .Include(a => a.AcknowledgedByLibrarian)
            .Where(a => a.IsActive && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.Priority)
            .ThenByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Alert> CreateAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);
        return alert;
    }

    public async Task<Alert> UpdateAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        _context.Alerts.Update(alert);
        await _context.SaveChangesAsync(cancellationToken);
        return alert;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var alert = await _context.Alerts.FindAsync(new object[] { id }, cancellationToken);
        if (alert != null)
        {
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Alerts.AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<int> CountActiveAlertsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .CountAsync(a => a.IsActive && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow), cancellationToken);
    }
}



