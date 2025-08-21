using AutoMapper;
using Library.Application.Abstractions.Services;
using Library.Application.Books.DTOs;
using MediatR;

namespace Library.Application.Books.Queries;

public record GetBookByIdQuery(int Id) : IRequest<BookResponseDto?>;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookResponseDto?>
{
    private readonly IBookService _service;
    private readonly IMapper _mapper;

    public GetBookByIdHandler(IBookService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<BookResponseDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _service.GetAsync(request.Id, cancellationToken);
        return book is null ? null : _mapper.Map<BookResponseDto>(book);
    }
}


