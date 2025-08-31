using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IAlertService
{
    Task<MemberAlertsDto> GetMyAlertsAsync(CancellationToken ct = default);
    Task<OverdueAlertsDto> GetOverdueAlertsAsync(CancellationToken ct = default);
    Task<FineAlertsDto> GetFineAlertsAsync(CancellationToken ct = default);
    Task<ReservationAlertsDto> GetReservationAlertsAsync(CancellationToken ct = default);
    Task<MembershipAlertsDto> GetMembershipAlertsAsync(CancellationToken ct = default);
    Task<AlertsSummaryDto> GetAlertsSummaryAsync(CancellationToken ct = default);
    Task DismissAlertAsync(int alertId, CancellationToken ct = default);
    Task DismissAllAlertsAsync(CancellationToken ct = default);
    Task<AlertSettingsDto> GetAlertSettingsAsync(CancellationToken ct = default);
    Task<AlertSettingsDto> UpdateAlertSettingsAsync(UpdateAlertSettingsRequest request, CancellationToken ct = default);
}

