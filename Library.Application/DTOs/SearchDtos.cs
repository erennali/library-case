namespace Library.Application.DTOs;

public record GlobalSearchRequest(
    string Query,
    int Page = 1,
    int PageSize = 10,
    string? FilterType = null
);

public record BookSearchRequest(
    string Query,
    int Page = 1,
    int PageSize = 10,
    int? CategoryId = null,
    string? Author = null,
    bool? AvailableOnly = false
);

public record MemberSearchRequest(
    string Query,
    int Page = 1,
    int PageSize = 10,
    string? MembershipType = null
);

public record AdvancedSearchRequest(
    string? Title = null,
    string? Author = null,
    string? Isbn = null,
    int? CategoryId = null,
    string? Publisher = null,
    int? PublicationYear = null,
    string? Language = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? Status = null,
    string? Format = null,
    int Page = 1,
    int PageSize = 10
);

public record SearchResultDto(
    string Query,
    int TotalResults,
    int Page,
    int PageSize,
    List<SearchResultItemDto> Items
);

public record SearchResultItemDto(
    int Id,
    string Type, // Book, Member, Author, etc.
    string Title,
    string Description,
    string? ImageUrl,
    Dictionary<string, object> Metadata
);

public record SaveSearchRequest(
    string Name,
    string Query,
    string? Description = null,
    bool IsDefault = false
);

public record SavedSearchDto(
    int Id,
    string Name,
    string Query,
    string? Description,
    bool IsDefault,
    DateTime CreatedAt,
    DateTime? LastUsed
);

public record SearchSuggestionDto(
    string Text,
    string Type,
    int Relevance
);

public record PopularSearchDto(
    string Query,
    int Count,
    DateTime LastSearched
);

public record GlobalSearchResultDto(
    string Query,
    int TotalResults,
    int Page,
    int PageSize,
    List<SearchResultItemDto> Results
);

public record BookSearchResultDto(
    string Query,
    int TotalResults,
    int Page,
    int PageSize,
    List<BookSearchItemDto> Results
);

public record MemberSearchResultDto(
    string Query,
    int TotalResults,
    int Page,
    int PageSize,
    List<MemberSearchItemDto> Results
);

public record AdvancedSearchResultDto(
    int TotalResults,
    int Page,
    int PageSize,
    List<BookSearchItemDto> Results,
    Dictionary<string, object> AppliedFilters
);

public record BookSearchItemDto(
    int Id,
    string ISBN,
    string Title,
    string Author,
    string? Publisher,
    int CategoryId,
    string CategoryName,
    int TotalCopies,
    int AvailableCopies,
    string? Language,
    int PageCount,
    decimal? Price,
    double? AverageRating,
    int ReviewCount,
    double RelevanceScore
);

public record MemberSearchItemDto(
    int Id,
    string MembershipNumber,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string MembershipType,
    string Status,
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    int CurrentBooksCount,
    decimal TotalFinesOwed,
    double RelevanceScore
);

public record RecentSearchDto(
    string Query,
    DateTime SearchedAt,
    string? Type,
    int ResultCount
);
