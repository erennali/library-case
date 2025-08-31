using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface INotificationService
{
    Task<NotificationResponseDto> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default);
    Task<BulkNotificationResponseDto> SendBulkAsync(SendBulkNotificationRequest request, CancellationToken ct = default);
    Task<NotificationResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<NotificationResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<NotificationResponseDto>> GetUnreadByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<NotificationResponseDto>> GetByTypeAsync(string type, int page, int pageSize, CancellationToken ct = default);
    Task MarkAsReadAsync(int id, CancellationToken ct = default);
    Task MarkMultipleAsReadAsync(MarkNotificationsReadRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<NotificationStatsDto> GetMemberStatsAsync(int memberId, CancellationToken ct = default);
    Task<PagedResult<NotificationResponseDto>> GetOverdueRemindersAsync(int page, int pageSize, CancellationToken ct = default);
}

