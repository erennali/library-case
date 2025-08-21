using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _dbContext;

    public BookRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetPagedAsync(int page, int pageSize, string? search, int? categoryId, CancellationToken cancellationToken = default)
    {
        IQueryable<Book> query = _dbContext.Books.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(b => b.Title.Contains(s) || b.Author.Contains(s) || b.ISBN.Contains(s));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId);
        }

        return await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(string? search, int? categoryId, CancellationToken cancellationToken = default)
    {
        IQueryable<Book> query = _dbContext.Books.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(b => b.Title.Contains(s) || b.Author.Contains(s) || b.ISBN.Contains(s));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _dbContext.Books.AddAsync(book, cancellationToken);
    }

    public void Update(Book book)
    {
        _dbContext.Books.Update(book);
    }

    public void Remove(Book book)
    {
        _dbContext.Books.Remove(book);
    }
}


