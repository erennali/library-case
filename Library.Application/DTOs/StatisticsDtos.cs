namespace Library.Application.DTOs;

public record StatisticsOverviewDto(
    int TotalBooks,
    int TotalMembers,
    int TotalTransactions,
    int OverdueItems,
    decimal TotalFines,
    int ActiveReservations,
    Dictionary<string, int> BooksByCategory,
    Dictionary<string, int> MembersByType
);

public record CirculationStatisticsDto(
    DateTime FromDate,
    DateTime ToDate,
    int TotalCheckouts,
    int TotalReturns,
    int TotalRenewals,
    int TotalOverdue,
    decimal TotalFines,
    double AverageBorrowDuration,
    List<DailyCirculationDto> DailyStats
);

public record TrendsStatisticsDto(
    string Period,
    List<TrendDataDto> Trends,
    Dictionary<string, List<decimal>> Metrics
);

public record ComparisonDto(
    DateTime Period1Start,
    DateTime Period1End,
    DateTime Period2Start,
    DateTime Period2End,
    Dictionary<string, ComparisonMetricDto> Metrics
);

public record ComparisonMetricDto(
    string MetricName,
    decimal Period1Value,
    decimal Period2Value,
    decimal Change,
    double ChangePercentage
);

public record TopBookDto(
    int BookId,
    string Title,
    string Author,
    int CheckoutCount,
    int ReservationCount,
    double AverageRating,
    int ReviewCount
);

public record TopMemberDto(
    int MemberId,
    string Name,
    int CheckoutCount,
    int ReturnCount,
    decimal TotalFines,
    DateTime LastActivity
);

public record BookStatisticsDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalBooks,
    int AvailableBooks,
    int CheckedOutBooks,
    int ReservedBooks,
    int OverdueBooks,
    double AverageRating,
    int TotalReviews,
    List<CategoryBookStatsDto> CategoryStats,
    List<LanguageBookStatsDto> LanguageStats
);

public record MemberStatisticsDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalMembers,
    int ActiveMembers,
    int NewMembers,
    int ExpiredMemberships,
    int MembersWithOverdue,
    int MembersWithFines,
    double AverageBooksPerMember,
    List<MembershipTypeStatsDto> MembershipTypeStats,
    List<AgeGroupStatsDto> AgeGroupStats
);

public record FineStatisticsDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalFines,
    int PendingFines,
    int PaidFines,
    int WaivedFines,
    decimal TotalAmount,
    decimal PendingAmount,
    decimal PaidAmount,
    decimal WaivedAmount,
    double AverageFineAmount,
    List<FineTypeStatsDto> FineTypeStats,
    List<MonthlyFineStatsDto> MonthlyStats
);

public record CategoryStatisticsDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalCategories,
    int ActiveCategories,
    Dictionary<string, int> BooksPerCategory,
    Dictionary<string, int> CheckoutsPerCategory,
    Dictionary<string, double> AverageRatingPerCategory,
    List<CategorySummaryDto> TopCategories
);

public record CategorySummaryDto(
    int Id,
    string Name,
    string? Description,
    int BookCount,
    int CheckoutCount,
    double AverageRating
);

public record LibrarianStatisticsDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalLibrarians,
    int ActiveLibrarians,
    int TotalTransactionsProcessed,
    int TotalReservationsProcessed,
    int TotalFinesProcessed,
    Dictionary<string, int> RoleDistribution,
    List<LibrarianPerformanceDto> TopPerformers
);



public record ComparisonStatisticsDto(
    DateTime Period1Start,
    DateTime Period1End,
    DateTime Period2Start,
    DateTime Period2End,
    string Metric,
    double Period1Value,
    double Period2Value,
    double Change,
    double ChangePercentage,
    string ChangeDirection
);

public record TopBookStatisticsDto(
    int BookId,
    string BookTitle,
    string Author,
    int CheckoutCount,
    int ReservationCount,
    double AverageRating,
    int ReviewCount,
    decimal? Price
);

public record TopMemberStatisticsDto(
    int MemberId,
    string MemberName,
    int CheckoutCount,
    int OverdueCount,
    decimal TotalFines,
    DateTime MembershipStartDate
);

public record OverdueAnalysisDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalOverdueBooks,
    int TotalOverdueMembers,
    decimal TotalFines,
    double AverageDaysOverdue,
    List<OverduePatternDto> OverduePatterns,
    List<MemberOverdueDto> TopOverdueMembers
);

public record FineAnalysisDto(
    DateTime? FromDate,
    DateTime? ToDate,
    int TotalFines,
    decimal TotalAmount,
    double AverageFineAmount,
    List<FinePatternDto> FinePatterns,
    List<MemberFineDto> TopFineMembers
);

public record CategoryBookStatsDto(
    int CategoryId,
    string CategoryName,
    int BookCount,
    int CheckoutCount,
    double AverageRating
);

public record LanguageBookStatsDto(
    string Language,
    int BookCount,
    int CheckoutCount,
    double AverageRating
);

public record MembershipTypeStatsDto(
    string MembershipType,
    int MemberCount,
    int ActiveCount,
    double AverageBooksPerMember
);

public record AgeGroupStatsDto(
    string AgeGroup,
    int MemberCount,
    int ActiveCount,
    double AverageBooksPerMember
);

public record FineTypeStatsDto(
    string FineType,
    int Count,
    decimal TotalAmount,
    double AverageAmount
);

public record MonthlyFineStatsDto(
    int Year,
    int Month,
    int Count,
    decimal TotalAmount
);

public record LibrarianPerformanceDto(
    int LibrarianId,
    string LibrarianName,
    int TransactionsProcessed,
    int ReservationsProcessed,
    int FinesProcessed,
    double AverageProcessingTime
);

public record OverduePatternDto(
    string Pattern,
    int Count,
    double Percentage
);

public record MemberOverdueDto(
    int MemberId,
    string MemberName,
    int OverdueCount,
    decimal TotalFines,
    double AverageDaysOverdue
);

public record FinePatternDto(
    string Pattern,
    int Count,
    decimal TotalAmount
);

public record MemberFineDto(
    int MemberId,
    string MemberName,
    int FineCount,
    decimal TotalAmount,
    double AverageAmount
);


