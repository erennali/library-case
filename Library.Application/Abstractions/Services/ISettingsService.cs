using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface ISettingsService
{
    Task<List<SettingResponseDto>> GetAllAsync(CancellationToken ct = default);
    Task<SettingResponseDto?> GetByKeyAsync(string key, CancellationToken ct = default);
    Task<SettingResponseDto> UpdateAsync(string key, UpdateSettingRequest request, CancellationToken ct = default);
    Task<List<SettingResponseDto>> UpdateBulkAsync(UpdateBulkSettingsRequest request, CancellationToken ct = default);
    Task DeleteAsync(string key, CancellationToken ct = default);
    Task<LibrarySettingsDto> GetLibrarySettingsAsync(CancellationToken ct = default);
    Task<LibrarySettingsDto> UpdateLibrarySettingsAsync(UpdateLibrarySettingsRequest request, CancellationToken ct = default);
    Task<FineSettingsDto> GetFineSettingsAsync(CancellationToken ct = default);
    Task<FineSettingsDto> UpdateFineSettingsAsync(UpdateFineSettingsRequest request, CancellationToken ct = default);
    Task<NotificationSettingsDto> GetNotificationSettingsAsync(CancellationToken ct = default);
    Task<NotificationSettingsDto> UpdateNotificationSettingsAsync(UpdateNotificationSettingsRequest request, CancellationToken ct = default);
    Task<SettingResponseDto> ResetToDefaultAsync(string key, CancellationToken ct = default);
    Task ResetAllToDefaultsAsync(CancellationToken ct = default);
}

