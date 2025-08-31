using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface ISearchService
{
    Task<GlobalSearchResultDto> GlobalSearchAsync(string query, int page, int pageSize, string? type, CancellationToken ct = default);
    Task<BookSearchResultDto> SearchBooksAsync(string query, int page, int pageSize, int? categoryId, string? author, string? language, bool? available, CancellationToken ct = default);
    Task<MemberSearchResultDto> SearchMembersAsync(string query, int page, int pageSize, string? membershipType, string? status, CancellationToken ct = default);
    Task<AdvancedSearchResultDto> AdvancedSearchAsync(string? title, string? author, string? isbn, int? categoryId, string? publisher, int? minPages, int? maxPages, decimal? minPrice, decimal? maxPrice, string? language, string? status, DateTime? publishedFrom, DateTime? publishedTo, int page, int pageSize, CancellationToken ct = default);
    Task<List<string>> GetSearchSuggestionsAsync(string query, string? type, int maxResults, CancellationToken ct = default);
    Task<List<PopularSearchDto>> GetPopularSearchesAsync(int maxResults, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<List<RecentSearchDto>> GetRecentSearchesAsync(int maxResults, CancellationToken ct = default);
    Task SaveSearchAsync(SaveSearchRequest request, CancellationToken ct = default);
    Task<List<SavedSearchDto>> GetSavedSearchesAsync(CancellationToken ct = default);
    Task DeleteSavedSearchAsync(int id, CancellationToken ct = default);
}

