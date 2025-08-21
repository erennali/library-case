using Library.Domain.Entities;

namespace Library.Application.Abstractions.Services;

public interface IBookService
{
    Task<Book?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Book> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, int? categoryId, CancellationToken cancellationToken = default);
    Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default);
    Task<Book> UpdateAsync(int id, Book updated, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}


