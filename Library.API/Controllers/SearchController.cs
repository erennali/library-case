using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("global")]
    public async Task<ActionResult<GlobalSearchResultDto>> GlobalSearch(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? type = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Search query is required" });

        try
        {
            var result = await _searchService.GlobalSearchAsync(query, page, pageSize, type, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("books")]
    public async Task<ActionResult<BookSearchResultDto>> SearchBooks(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? author = null,
        [FromQuery] string? language = null,
        [FromQuery] bool? available = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Search query is required" });

        try
        {
            var result = await _searchService.SearchBooksAsync(query, page, pageSize, categoryId, author, language, available, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("members")]
    public async Task<ActionResult<MemberSearchResultDto>> SearchMembers(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? membershipType = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Search query is required" });

        try
        {
            var result = await _searchService.SearchMembersAsync(query, page, pageSize, membershipType, status, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("advanced")]
    public async Task<ActionResult<AdvancedSearchResultDto>> AdvancedSearch(
        [FromQuery] string? title = null,
        [FromQuery] string? author = null,
        [FromQuery] string? isbn = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? publisher = null,
        [FromQuery] int? minPages = null,
        [FromQuery] int? maxPages = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? language = null,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? publishedFrom = null,
        [FromQuery] DateTime? publishedTo = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _searchService.AdvancedSearchAsync(
                title, author, isbn, categoryId, publisher, minPages, maxPages,
                minPrice, maxPrice, language, status, publishedFrom, publishedTo,
                page, pageSize, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("suggestions")]
    public async Task<ActionResult<List<string>>> GetSearchSuggestions(
        [FromQuery] string query,
        [FromQuery] string? type = null,
        [FromQuery] int maxResults = 10,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Search query is required" });

        try
        {
            var suggestions = await _searchService.GetSearchSuggestionsAsync(query, type, maxResults, ct);
            return Ok(suggestions);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("popular-searches")]
    public async Task<ActionResult<List<PopularSearchDto>>> GetPopularSearches(
        [FromQuery] int maxResults = 10,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var searches = await _searchService.GetPopularSearchesAsync(maxResults, fromDate, toDate, ct);
            return Ok(searches);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("recent-searches")]
    public async Task<ActionResult<List<RecentSearchDto>>> GetRecentSearches(
        [FromQuery] int maxResults = 10,
        CancellationToken ct = default)
    {
        try
        {
            var searches = await _searchService.GetRecentSearchesAsync(maxResults, ct);
            return Ok(searches);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveSearch([FromBody] SaveSearchRequest request, CancellationToken ct)
    {
        try
        {
            await _searchService.SaveSearchAsync(request, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("saved")]
    public async Task<ActionResult<List<SavedSearchDto>>> GetSavedSearches(CancellationToken ct)
    {
        try
        {
            var searches = await _searchService.GetSavedSearchesAsync(ct);
            return Ok(searches);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpDelete("saved/{id:int}")]
    public async Task<IActionResult> DeleteSavedSearch(int id, CancellationToken ct)
    {
        try
        {
            await _searchService.DeleteSavedSearchAsync(id, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}

