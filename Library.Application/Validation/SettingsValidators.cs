using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation;

public class UpdateSettingRequestValidator : AbstractValidator<UpdateSettingRequest>
{
    public UpdateSettingRequestValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Value is required and cannot exceed 500 characters");

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description cannot exceed 200 characters");
    }
}

