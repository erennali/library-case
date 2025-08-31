using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validation;

public class GenerateReportRequestValidator : AbstractValidator<GenerateReportRequest>
{
    public GenerateReportRequestValidator()
    {
        RuleFor(x => x.ReportType)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Report type is required and cannot exceed 100 characters");

        RuleFor(x => x.Format)
            .NotEmpty()
            .Must(x => new[] { "pdf", "excel", "csv", "json", "xml" }.Contains(x.ToLower()))
            .WithMessage("Format must be one of: pdf, excel, csv, json, xml");

        RuleFor(x => x.Parameters)
            .Must(x => x == null || x.Count <= 50)
            .WithMessage("Parameters cannot exceed 50 items");

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("From date must be less than or equal to to date");

        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("To date must be greater than or equal to from date");
    }
}

