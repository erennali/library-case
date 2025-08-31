using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation;

public class BorrowBookRequestValidator : AbstractValidator<BorrowBookRequest>
{
    public BorrowBookRequestValidator()
    {
        RuleFor(x => x.BookId)
            .GreaterThan(0)
            .WithMessage("Book ID must be greater than 0");

        RuleFor(x => x.MemberId)
            .GreaterThan(0)
            .WithMessage("Member ID must be greater than 0");

        RuleFor(x => x.Days)
            .InclusiveBetween(1, 90)
            .WithMessage("Days must be between 1 and 90");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

public class ReturnBookRequestValidator : AbstractValidator<ReturnBookRequest>
{
    public ReturnBookRequestValidator()
    {
        RuleFor(x => x.TransactionId)
            .GreaterThan(0)
            .WithMessage("Transaction ID must be greater than 0");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

public class RenewBookRequestValidator : AbstractValidator<RenewBookRequest>
{
    public RenewBookRequestValidator()
    {
        RuleFor(x => x.TransactionId)
            .GreaterThan(0)
            .WithMessage("Transaction ID must be greater than 0");

        RuleFor(x => x.AdditionalDays)
            .InclusiveBetween(1, 30)
            .WithMessage("Additional days must be between 1 and 30");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

