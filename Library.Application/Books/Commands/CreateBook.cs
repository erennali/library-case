using AutoMapper;
using FluentValidation;
using Library.Application.Abstractions.Services;
using Library.Application.Books.DTOs;
using Library.Application.Books.Validation;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Books.Commands;

public record CreateBookCommand(BookCreateDto Dto) : IRequest<BookResponseDto>;

public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookResponseDto>
{
    private readonly IBookService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<BookCreateDto> _validator;

    public CreateBookHandler(IBookService service, IMapper mapper, IValidator<BookCreateDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<BookResponseDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var entity = _mapper.Map<Book>(request.Dto);
        var created = await _service.CreateAsync(entity, cancellationToken);
        return _mapper.Map<BookResponseDto>(created);
    }
}


