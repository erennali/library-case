namespace Library.Application.DTOs;

public record DashboardOverviewDto(
    int TotalBooks,
    int AvailableBooks,
    int BorrowedBooks,
    int OverdueBooks,
    int TotalMembers,
    int ActiveMembers,
    int TotalTransactions,
    int OverdueTransactions,
    decimal TotalFines,
    int ActiveReservations,
    List<RecentActivityDto> RecentActivities,
    List<QuickActionDto> QuickActions
);

public record RecentActivityDto(
    int Id,
    string Type,
    string Description,
    string? UserName,
    DateTime Timestamp,
    string? Status
);

public record QuickActionDto(
    string Name,
    string Description,
    string Action,
    string Icon,
    bool IsEnabled
);

public record MemberActivitySummaryDto(
    int TotalMembers,
    int ActiveMembers,
    int NewMembersThisMonth,
    int ExpiredMemberships,
    List<MemberActivityDto> TopActiveMembers,
    Dictionary<string, int> MembersByType
);

public record MemberActivityDto(
    int MemberId,
    string Name,
    int CheckoutCount,
    int OverdueCount,
    decimal TotalFines,
    DateTime LastActivity
);

public record BookPopularitySummaryDto(
    int TotalBooks,
    int PopularBooks,
    int NewBooksThisMonth,
    List<BookPopularityDto> TopPopularBooks,
    Dictionary<string, int> BooksByCategory
);

public record BookPopularityDto(
    int BookId,
    string Title,
    string Author,
    int CheckoutCount,
    int ReservationCount,
    double AverageRating,
    int ReviewCount
);

public record LibrarianActivitySummaryDto(
    int TotalLibrarians,
    int ActiveLibrarians,
    int TotalTransactionsProcessed,
    List<LibrarianActivityDto> TopPerformers,
    Dictionary<string, int> LibrariansByRole
);

public record CirculationStatsDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalCheckouts,
    int TotalReturns,
    int TotalRenewals,
    int TotalOverdue,
    decimal TotalFines,
    double AverageLoanDuration,
    List<DailyCirculationDto> DailyStats
);

public record OverdueSummaryDto(
    int TotalOverdueBooks,
    int TotalOverdueMembers,
    decimal TotalFines,
    int CriticalOverdueCount,
    List<OverdueItemDto> TopOverdueItems
);

public record CategoryStatsDto(
    int TotalCategories,
    int ActiveCategories,
    Dictionary<string, int> BooksPerCategory,
    Dictionary<string, int> CheckoutsPerCategory,
    List<CategorySummaryDto> TopCategories
);

public record RecentTransactionDto(
    int Id,
    string TransactionNumber,
    string BookTitle,
    string MemberName,
    string Type,
    DateTime Date,
    string Status
);

public record RecentReservationDto(
    int Id,
    string ReservationNumber,
    string BookTitle,
    string MemberName,
    DateTime ReservationDate,
    DateTime ExpiryDate,
    string Status
);

public record SystemHealthDto(
    string DatabaseStatus,
    string StorageStatus,
    string EmailServiceStatus,
    string NotificationServiceStatus,
    DateTime LastCheck,
    List<HealthCheckDto> HealthChecks
);

public record TrendsDto(
    string TrendType,
    int Days,
    List<TrendDataDto> Data,
    string? GroupBy
);

public record HealthCheckDto(
    string Name,
    string Status,
    string Description,
    double Duration
);

public record OverdueItemDto(
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    int DaysOverdue,
    decimal FineAmount
);


