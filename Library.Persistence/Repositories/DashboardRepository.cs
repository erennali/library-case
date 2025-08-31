using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books.CountAsync(cancellationToken);
    }

    public async Task<int> GetTotalMembersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members.CountAsync(cancellationToken);
    }

    public async Task<int> GetTotalTransactionsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Transactions.CountAsync(cancellationToken);
    }

    public async Task<int> GetOverdueTransactionsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .CountAsync(t => t.Status == TransactionStatus.Overdue, cancellationToken);
    }

    public async Task<decimal> GetTotalFinesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Fines
            .Where(f => f.Status == FineStatus.Unpaid)
            .SumAsync(f => f.Amount, cancellationToken);
    }

    public async Task<int> GetActiveReservationsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .CountAsync(r => r.Status == ReservationStatus.Active, cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetPopularBooksAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Category)
            .OrderBy(b => b.AvailableCopies) // Most borrowed (least available)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Member>> GetTopMembersAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .OrderByDescending(m => m.CurrentBooksCount)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetBooksByCategoryAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Category)
            .GroupBy(b => b.Category.Name)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Category, x => x.Count, cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetTransactionsByMonthAsync(int year, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(t => t.CheckoutDate.Year == year)
            .GroupBy(t => t.CheckoutDate.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => GetMonthName(x.Month), x => x.Count, cancellationToken);
    }

    private static string GetMonthName(int month)
    {
        return month switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => "Unknown"
        };
    }
}



