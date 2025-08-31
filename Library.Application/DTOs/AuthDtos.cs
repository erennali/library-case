namespace Library.Application.DTOs;

public record LoginRequest(
    string Email,
    string Password,
    bool RememberMe = false
);

public record RegisterRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string? PhoneNumber = null,
    string UserType = "Member" // Member, Librarian
);

public record RefreshTokenRequest(
    string RefreshToken
);

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
);

public record ForgotPasswordRequest(
    string Email
);

public record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword
);

public record UpdateUserProfileRequest(
    string FirstName,
    string LastName,
    string? PhoneNumber = null
);

public record AuthResponseDto(
    bool Success,
    string? AccessToken = null,
    string? RefreshToken = null,
    string? Message = null,
    UserProfileDto? User = null,
    DateTime? ExpiresAt = null
);

public record UserProfileDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string UserType,
    string Status,
    List<string> Roles,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

public record TokenValidationResult(
    bool IsValid,
    int? UserId = null,
    string? Email = null,
    List<string>? Roles = null,
    DateTime? ExpiresAt = null
);

public record LogoutRequest(
    string? RefreshToken = null
);

public record ValidateTokenRequest(
    string Token
);
