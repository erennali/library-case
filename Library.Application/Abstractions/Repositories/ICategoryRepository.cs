using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> GetPagedAsync(int page, int pageSize, string? search, int? parentCategoryId, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? search, int? parentCategoryId, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    void Update(Category category);
    void Remove(Category category);
}


