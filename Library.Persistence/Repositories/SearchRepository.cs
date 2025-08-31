using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly AppDbContext _context;

    public SearchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Book> Items, int TotalCount)> SearchBooksAsync(string query, int page, int pageSize, string? category, string? author, CancellationToken cancellationToken = default)
    {
        var queryable = _context.Books
            .Include(b => b.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            queryable = queryable.Where(b =>
                b.Title.Contains(query) ||
                b.Author.Contains(query) ||
                b.ISBN.Contains(query) ||
                (b.Description != null && b.Description.Contains(query)));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            queryable = queryable.Where(b => b.Category.Name.Contains(category));
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            queryable = queryable.Where(b => b.Author.Contains(author));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Member> Items, int TotalCount)> SearchMembersAsync(string query, int page, int pageSize, string? membershipType, CancellationToken cancellationToken = default)
    {
        var queryable = _context.Members.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            queryable = queryable.Where(m =>
                m.FirstName.Contains(query) ||
                m.LastName.Contains(query) ||
                m.Email.Contains(query) ||
                m.MembershipNumber.Contains(query));
        }

        if (!string.IsNullOrWhiteSpace(membershipType))
        {
            queryable = queryable.Where(m => m.MembershipType.ToString().Contains(membershipType));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .OrderBy(m => m.FirstName)
            .ThenBy(m => m.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Librarian> Items, int TotalCount)> SearchLibrariansAsync(string query, int page, int pageSize, string? role, CancellationToken cancellationToken = default)
    {
        var queryable = _context.Librarians.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            queryable = queryable.Where(l =>
                l.FirstName.Contains(query) ||
                l.LastName.Contains(query) ||
                l.Email.Contains(query) ||
                l.EmployeeNumber.Contains(query));
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            queryable = queryable.Where(l => l.Role.ToString().Contains(role));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .OrderBy(l => l.FirstName)
            .ThenBy(l => l.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<SearchSuggestion>> GetSearchSuggestionsAsync(string query, int maxResults, CancellationToken cancellationToken = default)
    {
        var suggestions = new List<SearchSuggestion>();

        // Book titles
        var bookTitles = await _context.Books
            .Where(b => b.Title.Contains(query))
            .Select(b => new SearchSuggestion { Text = b.Title, Type = "Book", Relevance = 100 })
            .Take(maxResults / 3)
            .ToListAsync(cancellationToken);
        suggestions.AddRange(bookTitles);

        // Authors
        var authors = await _context.Books
            .Where(b => b.Author.Contains(query))
            .Select(b => new SearchSuggestion { Text = b.Author, Type = "Author", Relevance = 90 })
            .Distinct()
            .Take(maxResults / 3)
            .ToListAsync(cancellationToken);
        suggestions.AddRange(authors);

        // Categories
        var categories = await _context.Categories
            .Where(c => c.Name.Contains(query))
            .Select(c => new SearchSuggestion { Text = c.Name, Type = "Category", Relevance = 80 })
            .Take(maxResults / 3)
            .ToListAsync(cancellationToken);
        suggestions.AddRange(categories);

        return suggestions.OrderByDescending(s => s.Relevance).Take(maxResults).ToList();
    }

    public async Task LogSearchQueryAsync(string query, string? userId, string? userType, CancellationToken cancellationToken = default)
    {
        // In a real implementation, you would store search logs in a separate table
        // For now, we'll just return a completed task
        await Task.CompletedTask;
    }

    public async Task<IReadOnlyList<PopularSearch>> GetPopularSearchesAsync(int count, CancellationToken cancellationToken = default)
    {
        // In a real implementation, you would aggregate search logs
        // For now, return some mock data
        return new List<PopularSearch>
        {
            new() { Query = "programming", Count = 150, LastSearched = DateTime.UtcNow.AddDays(-1) },
            new() { Query = "fiction", Count = 120, LastSearched = DateTime.UtcNow.AddDays(-2) },
            new() { Query = "science", Count = 100, LastSearched = DateTime.UtcNow.AddDays(-3) }
        }.Take(count).ToList();
    }
}



