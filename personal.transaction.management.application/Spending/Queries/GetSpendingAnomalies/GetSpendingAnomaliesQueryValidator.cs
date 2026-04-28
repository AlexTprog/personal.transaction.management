using FluentValidation;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public class GetSpendingAnomaliesQueryValidator : AbstractValidator<GetSpendingAnomaliesQuery>
{
	public GetSpendingAnomaliesQueryValidator()
	{
		RuleFor(x => x.From)
			.NotEmpty()
			.When(x => x.From > DateOnly.FromDateTime(DateTime.Now))
			.WithMessage("The 'From' date cannot be in the future.");
		RuleFor(x => x.UserId).NotEmpty();
	}
}
