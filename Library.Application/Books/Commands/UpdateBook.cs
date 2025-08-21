using AutoMapper;
using FluentValidation;
using Library.Application.Abstractions.Services;
using Library.Application.Books.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Books.Commands;

public record UpdateBookCommand(int Id, BookUpdateDto Dto) : IRequest<BookResponseDto>;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookResponseDto>
{
    private readonly IBookService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<BookUpdateDto> _validator;

    public UpdateBookHandler(IBookService service, IMapper mapper, IValidator<BookUpdateDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<BookResponseDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var entity = _mapper.Map<Book>(request.Dto);
        var updated = await _service.UpdateAsync(request.Id, entity, cancellationToken);
        return _mapper.Map<BookResponseDto>(updated);
    }
}


