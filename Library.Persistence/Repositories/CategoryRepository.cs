using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetPagedAsync(int page, int pageSize, string? search, int? parentCategoryId, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> query = _dbContext.Categories.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(c => c.Name.Contains(s));
        }
        if (parentCategoryId.HasValue)
        {
            query = query.Where(c => c.ParentCategoryId == parentCategoryId);
        }
        return await query.OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(string? search, int? parentCategoryId, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> query = _dbContext.Categories.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(c => c.Name.Contains(s));
        }
        if (parentCategoryId.HasValue)
        {
            query = query.Where(c => c.ParentCategoryId == parentCategoryId);
        }
        return await query.CountAsync(cancellationToken);
    }

    public Task AddAsync(Category category, CancellationToken cancellationToken = default)
        => _dbContext.Categories.AddAsync(category, cancellationToken).AsTask();

    public void Update(Category category) => _dbContext.Categories.Update(category);

    public void Remove(Category category) => _dbContext.Categories.Remove(category);
}


