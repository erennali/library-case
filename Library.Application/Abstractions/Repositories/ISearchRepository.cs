using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface ISearchRepository
{
    Task<(IReadOnlyList<Book> Items, int TotalCount)> SearchBooksAsync(string query, int page, int pageSize, string? category, string? author, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Member> Items, int TotalCount)> SearchMembersAsync(string query, int page, int pageSize, string? membershipType, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Librarian> Items, int TotalCount)> SearchLibrariansAsync(string query, int page, int pageSize, string? role, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SearchSuggestion>> GetSearchSuggestionsAsync(string query, int maxResults, CancellationToken cancellationToken = default);
    Task LogSearchQueryAsync(string query, string? userId, string? userType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PopularSearch>> GetPopularSearchesAsync(int count, CancellationToken cancellationToken = default);
}

public class SearchSuggestion
{
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Book, Author, Category, etc.
    public int Relevance { get; set; }
}

public class PopularSearch
{
    public string Query { get; set; } = string.Empty;
    public int Count { get; set; }
    public DateTime LastSearched { get; set; }
}

