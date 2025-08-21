using Library.Domain.Enums;

namespace Library.Application.Members.DTOs;

public record MemberCreateDto(
    string MembershipNumber,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? Address,
    DateTime DateOfBirth,
    MembershipType MembershipType,
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    MemberStatus Status,
    int MaxBooksAllowed,
    int CurrentBooksCount,
    decimal TotalFinesOwed,
    decimal MaxFineLimit
);

public record MemberUpdateDto(
    string MembershipNumber,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? Address,
    DateTime DateOfBirth,
    MembershipType MembershipType,
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    MemberStatus Status,
    int MaxBooksAllowed,
    int CurrentBooksCount,
    decimal TotalFinesOwed,
    decimal MaxFineLimit
);

public record MemberResponseDto(
    int Id,
    string MembershipNumber,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? Address,
    DateTime DateOfBirth,
    MembershipType MembershipType,
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    MemberStatus Status,
    int MaxBooksAllowed,
    int CurrentBooksCount,
    decimal TotalFinesOwed,
    decimal MaxFineLimit,
    DateTime CreatedAt,
    DateTime UpdatedAt
);


