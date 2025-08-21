using FluentValidation;
using Library.Application.Members.DTOs;

namespace Library.Application.Members.Validation;

public class MemberCreateDtoValidator : AbstractValidator<MemberCreateDto>
{
    public MemberCreateDtoValidator()
    {
        RuleFor(x => x.MembershipNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.MaxBooksAllowed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CurrentBooksCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TotalFinesOwed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxFineLimit).GreaterThanOrEqualTo(0);
    }
}

public class MemberUpdateDtoValidator : AbstractValidator<MemberUpdateDto>
{
    public MemberUpdateDtoValidator()
    {
        RuleFor(x => x.MembershipNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.MaxBooksAllowed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CurrentBooksCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TotalFinesOwed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxFineLimit).GreaterThanOrEqualTo(0);
    }
}


