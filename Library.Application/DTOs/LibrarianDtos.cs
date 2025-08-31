using Library.Domain.Enums;

namespace Library.Application.DTOs;

public record CreateLibrarianRequest(
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    LibrarianRole Role,
    DateTime HireDate,
    string? Department
);

public record UpdateLibrarianRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    LibrarianRole Role,
    string? Department
);

public record ChangeLibrarianRoleRequest(
    LibrarianRole NewRole,
    string? Reason = null
);

public record LibrarianResponseDto(
    int Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    LibrarianRole Role,
    LibrarianStatus Status,
    DateTime HireDate,
    string? Department,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record LibrarianStatsDto(
    int TotalLibrarians,
    int ActiveLibrarians,
    int InactiveLibrarians,
    Dictionary<LibrarianRole, int> RoleDistribution,
    int TotalTransactionsProcessed,
    int TotalReservationsProcessed,
    int TotalFinesProcessed
);

public record LibrarianActivityDto(
    int Id,
    string Activity,
    string EntityType,
    int EntityId,
    string EntityName,
    DateTime ActivityDate,
    string? Notes
);


