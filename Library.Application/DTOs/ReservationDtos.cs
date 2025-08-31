using Library.Domain.Enums;

namespace Library.Application.DTOs;

public record CreateReservationRequest(
    int BookId,
    int MemberId,
    int Priority = 1,
    string? Notes = null
);

public record CancelReservationRequest(
    int ReservationId,
    string? Reason = null
);

public record ReservationResponseDto(
    int Id,
    string ReservationNumber,
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    DateTime ReservationDate,
    DateTime ExpiryDate,
    DateTime? NotifiedDate,
    ReservationStatus Status,
    int Priority,
    string? Notes,
    DateTime? FulfilledDate,
    int? ProcessedByLibrarianId,
    string? ProcessedByLibrarianName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ReservationSummaryDto(
    int Id,
    string ReservationNumber,
    string BookTitle,
    string MemberName,
    DateTime ReservationDate,
    DateTime ExpiryDate,
    ReservationStatus Status,
    int Priority
);

public record FulfillReservationRequest(
    string? Notes = null
);
