using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Include(n => n.Member)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .Include(n => n.Member)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(n =>
                n.Title.Contains(search) ||
                n.Message.Contains(search) ||
                n.Member.FirstName.Contains(search) ||
                n.Member.LastName.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .Where(n => n.MemberId == memberId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetByTypeAsync(NotificationType type, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .Include(n => n.Member)
            .Where(n => n.Type == type);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetByStatusAsync(NotificationStatus status, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .Include(n => n.Member)
            .Where(n => n.Status == status);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Notification> CreateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);
        return notification;
    }

    public async Task<Notification> UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync(cancellationToken);
        return notification;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications.FindAsync(new object[] { id }, cancellationToken);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications.AnyAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(n => ids.Contains(n.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountUnreadAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .CountAsync(n => n.MemberId == memberId && n.Status == NotificationStatus.Unread, cancellationToken);
    }
}



