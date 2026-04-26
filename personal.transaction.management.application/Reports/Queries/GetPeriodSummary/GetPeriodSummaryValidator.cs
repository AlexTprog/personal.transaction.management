using FluentValidation;

namespace personal.transaction.management.application.Reports.Queries.GetPeriodSummary;

public class GetPeriodSummaryValidator : AbstractValidator<GetPeriodSummaryQuery>
{
    public GetPeriodSummaryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.From).NotEmpty();

        RuleFor(x => x.To)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.From).WithMessage("To date must be on or after From date.");
    }
}
