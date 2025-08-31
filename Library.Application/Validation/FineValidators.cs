using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation;

public class PayFineRequestValidator : AbstractValidator<PayFineRequest>
{
    public PayFineRequestValidator()
    {
        RuleFor(x => x.FineId)
            .GreaterThan(0)
            .WithMessage("Fine ID must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Payment method is required and cannot exceed 50 characters");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.ReferenceNumber))
            .WithMessage("Reference number cannot exceed 100 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

public class WaiveFineRequestValidator : AbstractValidator<WaiveFineRequest>
{
    public WaiveFineRequestValidator()
    {
        RuleFor(x => x.FineId)
            .GreaterThan(0)
            .WithMessage("Fine ID must be greater than 0");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Reason is required and cannot exceed 200 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

