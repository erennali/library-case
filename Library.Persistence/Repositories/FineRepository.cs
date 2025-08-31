using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class FineRepository : IFineRepository
{
    private readonly AppDbContext _context;

    public FineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Fine?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fines
            .Include(f => f.Transaction)
            .Include(f => f.Member)
            .Include(f => f.ProcessedByLibrarian)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Fines
            .Include(f => f.Transaction)
            .Include(f => f.Member)
            .Include(f => f.ProcessedByLibrarian)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(f =>
                f.Member.FirstName.Contains(search) ||
                f.Member.LastName.Contains(search) ||
                f.Transaction.Book.Title.Contains(search) ||
                (f.ProcessedByLibrarian != null && f.ProcessedByLibrarian.FirstName.Contains(search)) ||
                (f.ProcessedByLibrarian != null && f.ProcessedByLibrarian.LastName.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Fines
            .Include(f => f.Transaction)
            .Include(f => f.ProcessedByLibrarian)
            .Where(f => f.MemberId == memberId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetByStatusAsync(FineStatus status, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Fines
            .Include(f => f.Transaction)
            .Include(f => f.Member)
            .Include(f => f.ProcessedByLibrarian)
            .Where(f => f.Status == status);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Fine> CreateAsync(Fine fine, CancellationToken cancellationToken = default)
    {
        _context.Fines.Add(fine);
        await _context.SaveChangesAsync(cancellationToken);
        return fine;
    }

    public async Task<Fine> UpdateAsync(Fine fine, CancellationToken cancellationToken = default)
    {
        _context.Fines.Update(fine);
        await _context.SaveChangesAsync(cancellationToken);
        return fine;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var fine = await _context.Fines.FindAsync(new object[] { id }, cancellationToken);
        if (fine != null)
        {
            _context.Fines.Remove(fine);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fines.AnyAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<decimal> GetTotalFinesByMemberAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Fines
            .Where(f => f.MemberId == memberId)
            .SumAsync(f => f.Amount, cancellationToken);
    }

    public async Task<int> CountUnpaidFinesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Fines
            .CountAsync(f => f.Status == FineStatus.Unpaid, cancellationToken);
    }

    public async Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetPendingAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Fines
            .Include(f => f.Transaction)
            .Include(f => f.Member)
            .Include(f => f.ProcessedByLibrarian)
            .Where(f => f.Status == FineStatus.Pending);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Fine> Items, int TotalCount)> GetOverdueAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Fines
            .Include(f => f.Transaction)
            .Include(f => f.Member)
            .Include(f => f.ProcessedByLibrarian)
            .Where(f => f.Status == FineStatus.Overdue);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}

