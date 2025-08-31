using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMemberRepository _memberRepository;
    private readonly ILibrarianRepository _librarianRepository;

    public AuthService(
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        IMemberRepository memberRepository,
        ILibrarianRepository librarianRepository)
    {
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _memberRepository = memberRepository;
        _librarianRepository = librarianRepository;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        // Try to find user in members first
        var member = await _memberRepository.GetByEmailAsync(request.Email, ct);
        if (member != null)
        {
            if (!_passwordHasher.VerifyPassword(request.Password, member.PasswordHash))
                return new AuthResponseDto(false, Message: "Invalid credentials");

            if (member.Status != MemberStatus.Active)
                return new AuthResponseDto(false, Message: "Account is not active");

            var userProfile = new UserProfileDto(
                member.Id,
                member.Email,
                member.FirstName,
                member.LastName,
                member.PhoneNumber,
                "Member",
                member.Status.ToString(),
                new List<string> { "Member" },
                member.CreatedAt,
                DateTime.UtcNow
            );

            var accessToken = _jwtService.GenerateAccessToken(userProfile);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto(true, accessToken, refreshToken, "Login successful", userProfile, DateTime.UtcNow.AddMinutes(60));
        }

        // Try to find user in librarians
        var librarian = await _librarianRepository.GetByEmailAsync(request.Email, ct);
        if (librarian != null)
        {
            if (!_passwordHasher.VerifyPassword(request.Password, librarian.PasswordHash))
                return new AuthResponseDto(false, Message: "Invalid credentials");

            if (librarian.Status != LibrarianStatus.Active)
                return new AuthResponseDto(false, Message: "Account is not active");

            var userProfile = new UserProfileDto(
                librarian.Id,
                librarian.Email,
                librarian.FirstName,
                librarian.LastName,
                librarian.PhoneNumber,
                "Librarian",
                librarian.Status.ToString(),
                new List<string> { librarian.Role.ToString() },
                librarian.CreatedAt,
                DateTime.UtcNow
            );

            var accessToken = _jwtService.GenerateAccessToken(userProfile);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto(true, accessToken, refreshToken, "Login successful", userProfile, DateTime.UtcNow.AddMinutes(60));
        }

        return new AuthResponseDto(false, Message: "Invalid credentials");
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        // Check if email already exists
        var existingMember = await _memberRepository.GetByEmailAsync(request.Email, ct);
        var existingLibrarian = await _librarianRepository.GetByEmailAsync(request.Email, ct);

        if (existingMember != null || existingLibrarian != null)
            return new AuthResponseDto(false, Message: "Email already registered");

        // Validate password
        if (request.Password != request.ConfirmPassword)
            return new AuthResponseDto(false, Message: "Passwords do not match");

        if (request.Password.Length < 8)
            return new AuthResponseDto(false, Message: "Password must be at least 8 characters long");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        if (request.UserType.ToLower() == "librarian")
        {
            var librarian = new Librarian
            {
                EmployeeNumber = GenerateEmployeeNumber(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Role = LibrarianRole.Assistant,
                Status = LibrarianStatus.Active,
                HireDate = DateTime.UtcNow,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdLibrarian = await _librarianRepository.CreateAsync(librarian, ct);

            var userProfile = new UserProfileDto(
                createdLibrarian.Id,
                createdLibrarian.Email,
                createdLibrarian.FirstName,
                createdLibrarian.LastName,
                createdLibrarian.PhoneNumber,
                "Librarian",
                createdLibrarian.Status.ToString(),
                new List<string> { createdLibrarian.Role.ToString() },
                createdLibrarian.CreatedAt,
                null
            );

            var accessToken = _jwtService.GenerateAccessToken(userProfile);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto(true, accessToken, refreshToken, "Registration successful", userProfile, DateTime.UtcNow.AddMinutes(60));
        }
        else
        {
            var member = new Member
            {
                MembershipNumber = GenerateMembershipNumber(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                MembershipType = MembershipType.Regular,
                Status = MemberStatus.Active,
                MembershipStartDate = DateTime.UtcNow,
                MembershipEndDate = DateTime.UtcNow.AddYears(1),
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdMember = await _memberRepository.CreateAsync(member, ct);

            var userProfile = new UserProfileDto(
                createdMember.Id,
                createdMember.Email,
                createdMember.FirstName,
                createdMember.LastName,
                createdMember.PhoneNumber,
                "Member",
                createdMember.Status.ToString(),
                new List<string> { "Member" },
                createdMember.CreatedAt,
                null
            );

            var accessToken = _jwtService.GenerateAccessToken(userProfile);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto(true, accessToken, refreshToken, "Registration successful", userProfile, DateTime.UtcNow.AddMinutes(60));
        }
    }

    public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        // This would typically involve validating the refresh token against a database
        // For now, we'll return an error indicating this needs to be implemented
        return Task.FromResult(new AuthResponseDto(false, Message: "Refresh token functionality not implemented"));
    }

    public Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
    {
        var result = _jwtService.ValidateToken(token);
        return Task.FromResult(result.IsValid);
    }

    public Task<bool> RevokeTokenAsync(string token, CancellationToken ct = default)
    {
        // This would typically involve adding the token to a blacklist
        // For now, we'll return true indicating success
        return Task.FromResult(true);
    }

    public Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default)
    {
        // This would typically involve validating the current user and updating the password
        // For now, we'll return false indicating this needs to be implemented
        return Task.FromResult(false);
    }

    public Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        // This would typically involve sending a password reset email
        // For now, we'll return false indicating this needs to be implemented
        return Task.FromResult(false);
    }

    public Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        // This would typically involve validating the reset token and updating the password
        // For now, we'll return false indicating this needs to be implemented
        return Task.FromResult(false);
    }

    public async Task<UserProfileDto> GetUserProfileAsync(int userId, CancellationToken ct = default)
    {
        // Try to find user in members first
        var member = await _memberRepository.GetByIdAsync(userId, ct);
        if (member != null)
        {
            return new UserProfileDto(
                member.Id,
                member.Email,
                member.FirstName,
                member.LastName,
                member.PhoneNumber,
                "Member",
                member.Status.ToString(),
                new List<string> { "Member" },
                member.CreatedAt,
                null
            );
        }

        // Try to find user in librarians
        var librarian = await _librarianRepository.GetByIdAsync(userId, ct);
        if (librarian != null)
        {
            return new UserProfileDto(
                librarian.Id,
                librarian.Email,
                librarian.FirstName,
                librarian.LastName,
                librarian.PhoneNumber,
                "Librarian",
                librarian.Status.ToString(),
                new List<string> { librarian.Role.ToString() },
                librarian.CreatedAt,
                null
            );
        }

        return null!;
    }

    public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileRequest request, CancellationToken ct = default)
    {
        // Try to find user in members first
        var member = await _memberRepository.GetByIdAsync(userId, ct);
        if (member != null)
        {
            member.FirstName = request.FirstName;
            member.LastName = request.LastName;
            member.PhoneNumber = request.PhoneNumber;
            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member, ct);
            return true;
        }

        // Try to find user in librarians
        var librarian = await _librarianRepository.GetByIdAsync(userId, ct);
        if (librarian != null)
        {
            librarian.FirstName = request.FirstName;
            librarian.LastName = request.LastName;
            librarian.PhoneNumber = request.PhoneNumber;
            librarian.UpdatedAt = DateTime.UtcNow;

            await _librarianRepository.UpdateAsync(librarian, ct);
            return true;
        }

        return false;
    }

    private string GenerateEmployeeNumber()
    {
        return $"EMP{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
    }

    private string GenerateMembershipNumber()
    {
        return $"MEM{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
    }
}

