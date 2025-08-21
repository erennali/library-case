using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> GetPagedAsync(int page, int pageSize, string? search, int? categoryId, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? search, int? categoryId, CancellationToken cancellationToken = default);
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    void Update(Book book);
    void Remove(Book book);
}


