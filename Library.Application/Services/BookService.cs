using Library.Application.Abstractions.Repositories;
using Library.Application.Abstractions.Services;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<Book?> GetAsync(int id, CancellationToken cancellationToken = default)
        => _bookRepository.GetByIdAsync(id, cancellationToken);

    public async Task<(IReadOnlyList<Book> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, int? categoryId, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var itemsTask = _bookRepository.GetPagedAsync(page, pageSize, search, categoryId, cancellationToken);
        var countTask = _bookRepository.CountAsync(search, categoryId, cancellationToken);

        await Task.WhenAll(itemsTask, countTask);
        return (await itemsTask, await countTask);
    }

    public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(book.ISBN))
            throw new ArgumentException("ISBN is required");
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Title is required");
        if (string.IsNullOrWhiteSpace(book.Author))
            throw new ArgumentException("Author is required");

        book.CreatedAt = DateTime.UtcNow;
        book.UpdatedAt = DateTime.UtcNow;

        await _bookRepository.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task<Book> UpdateAsync(int id, Book updated, CancellationToken cancellationToken = default)
    {
        var existing = await _bookRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            throw new KeyNotFoundException($"Book {id} not found");

        existing.ISBN = updated.ISBN;
        existing.Title = updated.Title;
        existing.Author = updated.Author;
        existing.Publisher = updated.Publisher;
        existing.PublicationDate = updated.PublicationDate;
        existing.Description = updated.Description;
        existing.CategoryId = updated.CategoryId;
        existing.TotalCopies = updated.TotalCopies;
        existing.AvailableCopies = updated.AvailableCopies;
        existing.Language = updated.Language;
        existing.PageCount = updated.PageCount;
        existing.ImageUrl = updated.ImageUrl;
        existing.Price = updated.Price;
        existing.Status = updated.Status;
        existing.UpdatedAt = DateTime.UtcNow;

        _bookRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _bookRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            return;

        _bookRepository.Remove(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}


