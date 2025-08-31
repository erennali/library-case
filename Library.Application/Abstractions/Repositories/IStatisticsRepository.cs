using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IStatisticsRepository
{
    Task<BookStatistics> GetBookStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<MemberStatistics> GetMemberStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<TransactionStatistics> GetTransactionStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<FineStatistics> GetFineStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<CategoryStatistics> GetCategoryStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MonthlyTrend>> GetMonthlyTrendsAsync(int year, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PopularBook>> GetPopularBooksAsync(int count, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
}

public class BookStatistics
{
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public int BorrowedBooks { get; set; }
    public int ReservedBooks { get; set; }
    public int OverdueBooks { get; set; }
    public int NewBooksThisPeriod { get; set; }
    public int RemovedBooksThisPeriod { get; set; }
}

public class MemberStatistics
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int NewMembersThisPeriod { get; set; }
    public int ExpiredMemberships { get; set; }
    public Dictionary<string, int> MembersByType { get; set; } = new();
}

public class TransactionStatistics
{
    public int TotalTransactions { get; set; }
    public int BorrowedThisPeriod { get; set; }
    public int ReturnedThisPeriod { get; set; }
    public int OverdueTransactions { get; set; }
    public double AverageBorrowDuration { get; set; }
}

public class FineStatistics
{
    public decimal TotalFines { get; set; }
    public decimal CollectedFines { get; set; }
    public decimal OutstandingFines { get; set; }
    public int TotalFineRecords { get; set; }
    public int PaidFineRecords { get; set; }
    public int UnpaidFineRecords { get; set; }
}

public class CategoryStatistics
{
    public Dictionary<string, int> BooksByCategory { get; set; } = new();
    public Dictionary<string, int> TransactionsByCategory { get; set; } = new();
    public Dictionary<string, double> AverageRatingByCategory { get; set; } = new();
}

public class MonthlyTrend
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int BooksBorrowed { get; set; }
    public int BooksReturned { get; set; }
    public int NewMembers { get; set; }
    public decimal FinesCollected { get; set; }
}

public class PopularBook
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
    public int ReservationCount { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}

