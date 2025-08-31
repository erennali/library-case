using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<AuthResponseDto> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
    Task<bool> RevokeTokenAsync(string token, CancellationToken ct = default);
    Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task<UserProfileDto> GetUserProfileAsync(int userId, CancellationToken ct = default);
    Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileRequest request, CancellationToken ct = default);
}

