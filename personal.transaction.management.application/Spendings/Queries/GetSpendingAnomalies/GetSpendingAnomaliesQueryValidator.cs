using FluentValidation;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public class GetSpendingAnomaliesQueryValidator : AbstractValidator<GetSpendingAnomaliesQuery>
{
	public GetSpendingAnomaliesQueryValidator()
	{
		RuleFor(x => x.From)
			.NotEmpty()
			.WithMessage("The 'From' date is required.")
			.Must(from => from <= DateOnly.FromDateTime(DateTime.Now))
			.WithMessage("The 'From' date cannot be in the future.");

		RuleFor(x => x.UserId).NotEmpty();
	}
}
