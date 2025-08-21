using Library.Domain.Entities;

namespace Library.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<Category?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Category> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, int? parentCategoryId, CancellationToken cancellationToken = default);
    Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default);
    Task<Category> UpdateAsync(int id, Category updated, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}


