using AutoMapper;
using Library.Application.Abstractions.Services;
using Library.Application.Books.DTOs;
using Library.Application.Common.Pagination;
using MediatR;

namespace Library.Application.Books.Queries;

public record SearchBooksQuery(int Page, int PageSize, string? Query, int? CategoryId) : IRequest<PagedResult<BookResponseDto>>;

public class SearchBooksHandler : IRequestHandler<SearchBooksQuery, PagedResult<BookResponseDto>>
{
    private readonly IBookService _service;
    private readonly IMapper _mapper;

    public SearchBooksHandler(IBookService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<PagedResult<BookResponseDto>> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _service.SearchAsync(request.Page, request.PageSize, request.Query, request.CategoryId, cancellationToken);
        var mapped = items.Select(_mapper.Map<BookResponseDto>).ToList();
        return new PagedResult<BookResponseDto>(mapped, total, request.Page, request.PageSize);
    }
}


