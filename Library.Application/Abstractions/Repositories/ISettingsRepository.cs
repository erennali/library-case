using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface ISettingsRepository
{
    Task<LibrarySettings?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<LibrarySettings?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LibrarySettings>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LibrarySettings> CreateAsync(LibrarySettings settings, CancellationToken cancellationToken = default);
    Task<LibrarySettings> UpdateAsync(LibrarySettings settings, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}

