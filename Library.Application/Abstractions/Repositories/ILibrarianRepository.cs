using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface ILibrarianRepository
{
    Task<Librarian?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Librarian?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Librarian?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Librarian> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, string? role, string? status, CancellationToken cancellationToken = default);
    Task<Librarian> CreateAsync(Librarian librarian, CancellationToken cancellationToken = default);
    Task<Librarian> UpdateAsync(Librarian librarian, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default);
}

