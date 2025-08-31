using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation;

public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.MemberId)
            .GreaterThan(0)
            .WithMessage("Member ID must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Title is required and cannot exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Message is required and cannot exceed 1000 characters");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid notification type");

        RuleFor(x => x.RelatedEntityId)
            .GreaterThan(0)
            .When(x => x.RelatedEntityId.HasValue)
            .WithMessage("Related entity ID must be greater than 0 when provided");

        RuleFor(x => x.RelatedEntityType)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.RelatedEntityType))
            .WithMessage("Related entity type cannot exceed 50 characters");
    }
}

public class SendBulkNotificationRequestValidator : AbstractValidator<SendBulkNotificationRequest>
{
    public SendBulkNotificationRequestValidator()
    {
        RuleFor(x => x.MemberIds)
            .NotEmpty()
            .WithMessage("Member IDs list cannot be empty");

        RuleFor(x => x.MemberIds)
            .Must(x => x.All(id => id > 0))
            .WithMessage("All member IDs must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Title is required and cannot exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Message is required and cannot exceed 1000 characters");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid notification type");

        RuleFor(x => x.RelatedEntityId)
            .GreaterThan(0)
            .When(x => x.RelatedEntityId.HasValue)
            .WithMessage("Related entity ID must be greater than 0 when provided");

        RuleFor(x => x.RelatedEntityType)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.RelatedEntityType))
            .WithMessage("Related entity type cannot exceed 50 characters");
    }
}

