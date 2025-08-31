using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation;

public class CreateLibrarianRequestValidator : AbstractValidator<CreateLibrarianRequest>
{
    public CreateLibrarianRequestValidator()
    {
        RuleFor(x => x.EmployeeNumber)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Employee number is required and cannot exceed 50 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("First name is required and cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Last name is required and cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200)
            .WithMessage("Valid email is required and cannot exceed 200 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid librarian role");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Hire date cannot be in the future");

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Department))
            .WithMessage("Department cannot exceed 100 characters");
    }
}

public class UpdateLibrarianRequestValidator : AbstractValidator<UpdateLibrarianRequest>
{
    public UpdateLibrarianRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("First name is required and cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Last name is required and cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200)
            .WithMessage("Valid email is required and cannot exceed 200 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid librarian role");

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Department))
            .WithMessage("Department cannot exceed 100 characters");
    }
}

