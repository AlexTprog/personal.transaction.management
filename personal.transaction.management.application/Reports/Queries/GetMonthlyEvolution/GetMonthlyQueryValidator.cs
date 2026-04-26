using FluentValidation;

namespace personal.transaction.management.application.Reports.Queries.GetMonthlyEvolution;

public class GetMonthlyQueryValidator : AbstractValidator<GetMonthlyEvolutionQuery>
{
	public GetMonthlyQueryValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty().WithMessage("UserId is required.");

		RuleFor(x => x.Date)
			.NotEmpty().WithMessage("Date is required.")
			.Must(date => date.Year >= 2000 && date.Year <= DateTime.Now.Year)
			.WithMessage($"Year must be between 2000 and {DateTime.Now.Year}.");

		RuleFor(x => x.LastMonths)
			.GreaterThan(0).WithMessage("LastMonths must be greater than 0.")
			.LessThanOrEqualTo(120).WithMessage("LastMonths cannot exceed 120.");
	}
}
