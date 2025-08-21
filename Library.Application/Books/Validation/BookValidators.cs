using FluentValidation;
using Library.Application.Books.DTOs;

namespace Library.Application.Books.Validation;

public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
{
    public BookCreateDtoValidator()
    {
        RuleFor(x => x.ISBN).NotEmpty().Length(10, 13);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(300);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.TotalCopies).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AvailableCopies).GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.TotalCopies);
        RuleFor(x => x.PageCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
    }
}

public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
{
    public BookUpdateDtoValidator()
    {
        RuleFor(x => x.ISBN).NotEmpty().Length(10, 13);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(300);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.TotalCopies).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AvailableCopies).GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.TotalCopies);
        RuleFor(x => x.PageCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
    }
}


