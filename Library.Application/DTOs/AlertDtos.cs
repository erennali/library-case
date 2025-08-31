namespace Library.Application.DTOs;

public record CreateAlertRequest(
    string Title,
    string Message,
    string AlertType,
    string Severity,
    DateTime? ExpiresAt = null,
    string? Source = null,
    int Priority = 1,
    string? AdditionalData = null
);

public record UpdateAlertRequest(
    string? Title = null,
    string? Message = null,
    string? AlertType = null,
    string? Severity = null,
    bool? IsActive = null,
    DateTime? ExpiresAt = null,
    int? Priority = null,
    string? AdditionalData = null
);

public record AlertResponseDto(
    int Id,
    string Title,
    string Message,
    string AlertType,
    string Severity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    DateTime? AcknowledgedAt,
    string? AcknowledgedByLibrarianName,
    string? AdditionalData,
    string? Source,
    int Priority
);

public record AlertDto(
    int Id,
    string Title,
    string Message,
    string AlertType,
    string Severity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    DateTime? AcknowledgedAt,
    string? AcknowledgedByLibrarianName,
    string? AdditionalData,
    string? Source,
    int Priority
);

public record AlertSummaryDto(
    int TotalAlerts,
    int ActiveAlerts,
    int CriticalAlerts,
    int HighPriorityAlerts,
    int UnacknowledgedAlerts
);

public record AcknowledgeAlertRequest(
    string? Notes = null
);

public record DismissAlertRequest(
    string? Reason = null
);

public record AlertSettingsDto(
    bool EnableEmailNotifications,
    bool EnableSmsNotifications,
    bool EnablePushNotifications,
    List<string> AlertTypes,
    Dictionary<string, string> SeverityColors
);

public record UpdateAlertSettingsRequest(
    bool? EnableEmailNotifications = null,
    bool? EnableSmsNotifications = null,
    bool? EnablePushNotifications = null,
    List<string>? AlertTypes = null,
    Dictionary<string, string>? SeverityColors = null
);

public record MemberAlertsDto(
    int MemberId,
    string MemberName,
    int OverdueCount,
    int FineCount,
    int ReservationCount,
    int MembershipAlertCount,
    List<AlertItemDto> Alerts
);

public record OverdueAlertsDto(
    int TotalOverdue,
    decimal TotalFines,
    List<OverdueAlertDto> OverdueItems
);

public record FineAlertsDto(
    int TotalFines,
    decimal TotalAmount,
    List<FineAlertDto> FineItems
);

public record ReservationAlertsDto(
    int TotalReservations,
    int ExpiredReservations,
    List<ReservationAlertDto> ReservationItems
);

public record MembershipAlertsDto(
    int ExpiringMemberships,
    int ExpiredMemberships,
    List<MembershipAlertDto> MembershipItems
);

public record AlertsSummaryDto(
    int TotalAlerts,
    int UnreadAlerts,
    int OverdueAlerts,
    int FineAlerts,
    int ReservationAlerts,
    int MembershipAlerts
);

public record AlertItemDto(
    int Id,
    string Type,
    string Title,
    string Message,
    DateTime CreatedAt,
    bool IsRead
);

public record OverdueAlertDto(
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    int DaysOverdue,
    decimal FineAmount
);

public record FineAlertDto(
    int FineId,
    int MemberId,
    string MemberName,
    decimal Amount,
    DateTime DueDate
);

public record ReservationAlertDto(
    int ReservationId,
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    DateTime ExpiryDate,
    bool IsExpired
);

public record MembershipAlertDto(
    int MemberId,
    string MemberName,
    DateTime MembershipEndDate,
    int DaysUntilExpiry,
    bool IsExpired
);
