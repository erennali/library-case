using Library.Domain.Enums;

namespace Library.Application.DTOs;

public record BorrowBookRequest(
    int BookId,
    int MemberId,
    int Days = 14,
    string? Notes = null
);

public record ReturnBookRequest(
    int TransactionId,
    string? Notes = null
);

public record RenewBookRequest(
    int TransactionId,
    int AdditionalDays = 14,
    string? Notes = null
);

public record TransactionResponseDto(
    int Id,
    string TransactionNumber,
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    TransactionType Type,
    DateTime CheckoutDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    TransactionStatus Status,
    int? RenewalCount,
    decimal? FineAmount,
    string? Notes,
    int? ProcessedByLibrarianId,
    string? ProcessedByLibrarianName,
    DateTime CreatedAt
);

public record TransactionSummaryDto(
    int Id,
    string TransactionNumber,
    string BookTitle,
    string MemberName,
    TransactionType Type,
    DateTime CheckoutDate,
    DateTime DueDate,
    TransactionStatus Status,
    decimal? FineAmount
);

public record TransactionStatsDto(
    int TotalTransactions,
    int ActiveTransactions,
    int OverdueTransactions,
    int CompletedTransactions,
    decimal TotalFines,
    double AverageBorrowDuration
);
