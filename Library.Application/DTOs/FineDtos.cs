using Library.Domain.Enums;

namespace Library.Application.DTOs;

public record PayFineRequest(
    int FineId,
    decimal Amount,
    string PaymentMethod,
    string? ReferenceNumber = null,
    string? Notes = null
);

public record WaiveFineRequest(
    int FineId,
    string Reason,
    string? Notes = null
);

public record FineResponseDto(
    int Id,
    string FineNumber,
    int TransactionId,
    string TransactionNumber,
    int MemberId,
    string MemberName,
    FineType Type,
    decimal Amount,
    DateTime IssueDate,
    DateTime? DueDate,
    FineStatus Status,
    DateTime? PaidDate,
    decimal? PaidAmount,
    string? Description,
    string? Notes,
    int? ProcessedByLibrarianId,
    string? ProcessedByLibrarianName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record FineSummaryDto(
    int MemberId,
    string MemberName,
    int TotalFines,
    int PendingFines,
    int PaidFines,
    decimal TotalAmount,
    decimal PendingAmount,
    decimal PaidAmount
);

public record OverallFineSummaryDto(
    int TotalFines,
    int PendingFines,
    int PaidFines,
    decimal TotalAmount,
    decimal PendingAmount,
    decimal PaidAmount,
    decimal AverageFineAmount
);

public record MemberFineSummaryDto(
    int MemberId,
    string MemberName,
    int TotalFines,
    int PendingFines,
    int PaidFines,
    decimal TotalAmount,
    decimal PendingAmount,
    decimal PaidAmount
);
