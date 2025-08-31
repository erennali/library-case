using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly AppDbContext _context;

    public StatisticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookStatistics> GetBookStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var totalBooks = await _context.Books.CountAsync(cancellationToken);
        var availableBooks = await _context.Books.Where(b => b.Status == BookStatus.Available).CountAsync(cancellationToken);
        var borrowedBooks = await _context.Books.Where(b => b.Status == BookStatus.Borrowed).CountAsync(cancellationToken);
        var reservedBooks = await _context.Books.Where(b => b.Status == BookStatus.Reserved).CountAsync(cancellationToken);
        var overdueBooks = await _context.Transactions
            .Where(t => t.Status == TransactionStatus.Borrowed && t.DueDate < DateTime.UtcNow)
            .CountAsync(cancellationToken);

        var newBooks = await _context.Books
            .Where(b => b.CreatedAt >= fromDate && b.CreatedAt <= toDate)
            .CountAsync(cancellationToken);

        return new BookStatistics
        {
            TotalBooks = totalBooks,
            AvailableBooks = availableBooks,
            BorrowedBooks = borrowedBooks,
            ReservedBooks = reservedBooks,
            OverdueBooks = overdueBooks,
            NewBooksThisPeriod = newBooks,
            RemovedBooksThisPeriod = 0 // Would need to track deletions
        };
    }

    public async Task<MemberStatistics> GetMemberStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var totalMembers = await _context.Members.CountAsync(cancellationToken);
        var activeMembers = await _context.Members.Where(m => m.Status == MemberStatus.Active).CountAsync(cancellationToken);
        var newMembers = await _context.Members
            .Where(m => m.CreatedAt >= fromDate && m.CreatedAt <= toDate)
            .CountAsync(cancellationToken);
        var expiredMemberships = await _context.Members
            .Where(m => m.MembershipEndDate < DateTime.UtcNow)
            .CountAsync(cancellationToken);

        var membersByType = await _context.Members
            .GroupBy(m => m.MembershipType)
            .ToDictionaryAsync(g => g.Key.ToString(), g => g.Count(), cancellationToken);

        return new MemberStatistics
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            NewMembersThisPeriod = newMembers,
            ExpiredMemberships = expiredMemberships,
            MembersByType = membersByType
        };
    }

    public async Task<TransactionStatistics> GetTransactionStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var transactions = await _context.Transactions
            .Where(t => t.CheckoutDate >= fromDate && t.CheckoutDate <= toDate)
            .ToListAsync(cancellationToken);

        var totalTransactions = transactions.Count;
        var borrowedThisPeriod = transactions.Count(t => t.Type == TransactionType.Checkout);
        var returnedThisPeriod = transactions.Count(t => t.Type == TransactionType.Return);
        var overdueTransactions = await _context.Transactions
            .Where(t => t.Status == TransactionStatus.Borrowed && t.DueDate < DateTime.UtcNow)
            .CountAsync(cancellationToken);

        var avgDuration = transactions
            .Where(t => t.ReturnDate.HasValue)
            .Select(t => (t.ReturnDate!.Value - t.CheckoutDate).TotalDays)
            .DefaultIfEmpty(0)
            .Average();

        return new TransactionStatistics
        {
            TotalTransactions = totalTransactions,
            BorrowedThisPeriod = borrowedThisPeriod,
            ReturnedThisPeriod = returnedThisPeriod,
            OverdueTransactions = overdueTransactions,
            AverageBorrowDuration = avgDuration
        };
    }

    public async Task<FineStatistics> GetFineStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var fines = await _context.Fines
            .Where(f => f.CreatedAt >= fromDate && f.CreatedAt <= toDate)
            .ToListAsync(cancellationToken);

        return new FineStatistics
        {
            TotalFines = fines.Sum(f => f.Amount),
            CollectedFines = fines.Where(f => f.Status == FineStatus.Paid).Sum(f => f.Amount),
            OutstandingFines = fines.Where(f => f.Status == FineStatus.Unpaid).Sum(f => f.Amount),
            TotalFineRecords = fines.Count,
            PaidFineRecords = fines.Count(f => f.Status == FineStatus.Paid),
            UnpaidFineRecords = fines.Count(f => f.Status == FineStatus.Unpaid)
        };
    }

    public async Task<CategoryStatistics> GetCategoryStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var booksByCategory = await _context.Books
            .Include(b => b.Category)
            .GroupBy(b => b.Category.Name)
            .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);

        var transactionsByCategory = await _context.Transactions
            .Include(t => t.Book)
            .ThenInclude(b => b.Category)
            .Where(t => t.CheckoutDate >= fromDate && t.CheckoutDate <= toDate)
            .GroupBy(t => t.Book.Category.Name)
            .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);

        // Mock average rating by category (would need Reviews table)
        var avgRatingByCategory = booksByCategory.ToDictionary(
            kvp => kvp.Key,
            kvp => 4.2); // Mock rating

        return new CategoryStatistics
        {
            BooksByCategory = booksByCategory,
            TransactionsByCategory = transactionsByCategory,
            AverageRatingByCategory = avgRatingByCategory
        };
    }

    public async Task<IReadOnlyList<MonthlyTrend>> GetMonthlyTrendsAsync(int year, CancellationToken cancellationToken = default)
    {
        var trends = new List<MonthlyTrend>();

        for (int month = 1; month <= 12; month++)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var borrowed = await _context.Transactions
                .Where(t => t.CheckoutDate >= startDate && t.CheckoutDate <= endDate && t.Type == TransactionType.Checkout)
                .CountAsync(cancellationToken);

            var returned = await _context.Transactions
                .Where(t => t.ReturnDate >= startDate && t.ReturnDate <= endDate && t.Type == TransactionType.Return)
                .CountAsync(cancellationToken);

            var newMembers = await _context.Members
                .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate)
                .CountAsync(cancellationToken);

            var finesCollected = await _context.Fines
                .Where(f => f.PaidDate >= startDate && f.PaidDate <= endDate && f.Status == FineStatus.Paid)
                .SumAsync(f => f.Amount, cancellationToken);

            trends.Add(new MonthlyTrend
            {
                Month = month,
                Year = year,
                BooksBorrowed = borrowed,
                BooksReturned = returned,
                NewMembers = newMembers,
                FinesCollected = finesCollected
            });
        }

        return trends;
    }

    public async Task<IReadOnlyList<PopularBook>> GetPopularBooksAsync(int count, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var popularBooks = await _context.Transactions
            .Include(t => t.Book)
            .Where(t => t.CheckoutDate >= fromDate && t.CheckoutDate <= toDate)
            .GroupBy(t => new { t.BookId, t.Book.Title, t.Book.Author })
            .Select(g => new PopularBook
            {
                BookId = g.Key.BookId,
                Title = g.Key.Title,
                Author = g.Key.Author,
                BorrowCount = g.Count(),
                ReservationCount = 0, // Would need to calculate from reservations
                AverageRating = 4.2, // Mock rating (would calculate from reviews)
                ReviewCount = 0 // Would calculate from reviews
            })
            .OrderByDescending(b => b.BorrowCount)
            .Take(count)
            .ToListAsync(cancellationToken);

        return popularBooks;
    }
}


