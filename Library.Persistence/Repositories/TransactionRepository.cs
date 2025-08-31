using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(t => t.Book)
            .Include(t => t.Member)
            .Include(t => t.ProcessedByLibrarian)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Include(t => t.Book)
            .Include(t => t.Member)
            .Include(t => t.ProcessedByLibrarian)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.Book.Title.Contains(search) ||
                t.Member.FirstName.Contains(search) ||
                t.Member.LastName.Contains(search) ||
                (t.ProcessedByLibrarian != null && t.ProcessedByLibrarian.FirstName.Contains(search)) ||
                (t.ProcessedByLibrarian != null && t.ProcessedByLibrarian.LastName.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(t => t.CheckoutDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Include(t => t.Book)
            .Include(t => t.ProcessedByLibrarian)
            .Where(t => t.MemberId == memberId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(t => t.CheckoutDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Include(t => t.Member)
            .Include(t => t.ProcessedByLibrarian)
            .Where(t => t.BookId == bookId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(t => t.CheckoutDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetOverdueAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Include(t => t.Book)
            .Include(t => t.Member)
            .Include(t => t.ProcessedByLibrarian)
            .Where(t => t.Status == TransactionStatus.Borrowed && t.DueDate < DateTime.UtcNow);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(t => t.DueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetActiveAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Include(t => t.Book)
            .Include(t => t.Member)
            .Include(t => t.ProcessedByLibrarian)
            .Where(t => t.Status == TransactionStatus.Borrowed);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(t => t.CheckoutDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction;
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions.FindAsync(new object[] { id }, cancellationToken);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions.AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<int> CountOverdueAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .CountAsync(t => t.Status == TransactionStatus.Borrowed && t.DueDate < DateTime.UtcNow, cancellationToken);
    }

    public async Task<decimal> GetTotalFinesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(t => t.FineAmount.HasValue)
            .SumAsync(t => t.FineAmount.Value, cancellationToken);
    }
}

