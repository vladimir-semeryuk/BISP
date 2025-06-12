using FluentValidation;

namespace EchoesOfUzbekistan.Application.Reports.CreateInappropriateContentReport;
public class CreateInappropriateContentReportCommandValidator : AbstractValidator<CreateInappropriateContentReportCommand>
{
    public CreateInappropriateContentReportCommandValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason must be provided.")
            .MinimumLength(10).WithMessage("Be more explicit expressing the reason.");
    }
}