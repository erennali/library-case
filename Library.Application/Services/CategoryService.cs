using Library.Application.Abstractions.Repositories;
using Library.Application.Abstractions.Services;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public CategoryService(ICategoryRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public Task<Category?> GetAsync(int id, CancellationToken cancellationToken = default)
        => _repo.GetByIdAsync(id, cancellationToken);

    public async Task<(IReadOnlyList<Category> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, int? parentCategoryId, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        var itemsTask = _repo.GetPagedAsync(page, pageSize, search, parentCategoryId, cancellationToken);
        var countTask = _repo.CountAsync(search, parentCategoryId, cancellationToken);
        await Task.WhenAll(itemsTask, countTask);
        return (await itemsTask, await countTask);
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category.Name)) throw new ArgumentException("Name is required");
        await _repo.AddAsync(category, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<Category> UpdateAsync(int id, Category updated, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) throw new KeyNotFoundException($"Category {id} not found");
        existing.Name = updated.Name;
        existing.Description = updated.Description;
        existing.ParentCategoryId = updated.ParentCategoryId;
        existing.IsActive = updated.IsActive;
        _repo.Update(existing);
        await _uow.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) return;
        _repo.Remove(existing);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}


