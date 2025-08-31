using Library.Domain.Enums;

namespace Library.Application.DTOs;

public record CreateNotificationRequest(
    int MemberId,
    string Title,
    string Message,
    NotificationType Type,
    int? RelatedEntityId = null,
    string? RelatedEntityType = null
);

public record SendBulkNotificationRequest(
    List<int> MemberIds,
    string Title,
    string Message,
    NotificationType Type,
    int? RelatedEntityId = null,
    string? RelatedEntityType = null
);

public record MarkNotificationsReadRequest(
    List<int> NotificationIds
);

public record NotificationResponseDto(
    int Id,
    int MemberId,
    string MemberName,
    string Title,
    string Message,
    NotificationType Type,
    NotificationStatus Status,
    DateTime CreatedAt,
    DateTime? ReadAt,
    int? RelatedEntityId,
    string? RelatedEntityType,
    bool IsEmailSent,
    DateTime? EmailSentAt
);

public record BulkNotificationResponseDto(
    int TotalSent,
    int SuccessCount,
    int FailureCount,
    List<string> Errors
);

public record NotificationStatsDto(
    int MemberId,
    string MemberName,
    int TotalNotifications,
    int UnreadCount,
    int ReadCount,
    DateTime? LastNotificationDate
);

public record UpdateNotificationRequest(
    string? Title = null,
    string? Message = null,
    NotificationType? Type = null,
    int? RelatedEntityId = null,
    string? RelatedEntityType = null
);

public record NotificationSummaryDto(
    int TotalNotifications,
    int UnreadNotifications,
    int ReadNotifications,
    Dictionary<string, int> NotificationsByType,
    Dictionary<string, int> NotificationsByStatus
);

public record MarkMultipleNotificationsReadRequest(
    List<int> NotificationIds
);
