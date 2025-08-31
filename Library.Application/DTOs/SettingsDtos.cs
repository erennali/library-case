namespace Library.Application.DTOs;

public record UpdateSettingRequest(
    string Value,
    string? Description = null
);

public record UpdateBulkSettingsRequest(
    Dictionary<string, UpdateSettingRequest> Settings
);

public record SettingResponseDto(
    int Id,
    string Key,
    string Value,
    string? Description,
    DateTime UpdatedAt,
    int? UpdatedByLibrarianId,
    string? UpdatedByLibrarianName
);

public record LibrarySettingsDto(
    string LibraryName,
    string LibraryAddress,
    string LibraryPhone,
    string LibraryEmail,
    string LibraryWebsite,
    string Currency,
    int MaxBooksPerMember,
    int DefaultLoanDays,
    int MaxRenewals,
    decimal DailyFineAmount,
    int MaxFineLimit,
    bool AllowReservations,
    int ReservationExpiryDays,
    bool RequireReviewApproval
);

public record UpdateLibrarySettingsRequest(
    string LibraryName,
    string LibraryAddress,
    string LibraryPhone,
    string LibraryEmail,
    string LibraryWebsite,
    string Currency,
    int MaxBooksPerMember,
    int DefaultLoanDays,
    int MaxRenewals,
    decimal DailyFineAmount,
    int MaxFineLimit,
    bool AllowReservations,
    int ReservationExpiryDays,
    bool RequireReviewApproval
);

public record FineSettingsDto(
    decimal DailyFineAmount,
    int MaxFineLimit,
    bool AllowFineWaiver,
    bool SendFineReminders,
    int ReminderFrequencyDays,
    bool BlockMembershipOnMaxFine,
    List<string> FineTypes
);

public record UpdateFineSettingsRequest(
    decimal DailyFineAmount,
    int MaxFineLimit,
    bool AllowFineWaiver,
    bool SendFineReminders,
    int ReminderFrequencyDays,
    bool BlockMembershipOnMaxFine,
    List<string> FineTypes
);

public record NotificationSettingsDto(
    bool EmailNotifications,
    bool PushNotifications,
    bool SMSNotifications,
    bool OverdueReminders,
    bool FineReminders,
    bool ReservationReminders,
    bool MembershipReminders,
    int ReminderFrequencyDays,
    string? SMTPHost,
    int? SMTPPort,
    string? SMTPUsername,
    string? SMTPPassword,
    bool SMTPUseSSL
);

public record UpdateNotificationSettingsRequest(
    bool EmailNotifications,
    bool PushNotifications,
    bool SMSNotifications,
    bool OverdueReminders,
    bool FineReminders,
    bool ReservationReminders,
    bool MembershipReminders,
    int ReminderFrequencyDays,
    string? SMTPHost,
    int? SMTPPort,
    string? SMTPUsername,
    string? SMTPPassword,
    bool SMTPUseSSL
);

public record SystemSettingsDto(
    string SystemName,
    string Version,
    string Environment,
    bool MaintenanceMode,
    string? MaintenanceMessage,
    DateTime LastBackup,
    bool AutoBackup,
    int BackupRetentionDays
);

public record UpdateSystemSettingsRequest(
    string SystemName,
    string Version,
    string Environment,
    bool MaintenanceMode,
    string? MaintenanceMessage,
    bool AutoBackup,
    int BackupRetentionDays
);
