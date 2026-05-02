using FluentValidation;
using personal.transaction.management.application.Common.Interfaces;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public class GetSpendingAnomaliesQueryValidator : AbstractValidator<GetSpendingAnomaliesQuery>
{
	public GetSpendingAnomaliesQueryValidator(IConfigurationService configurationService)
	{
		RuleFor(x => x.UserId)
			.NotEmpty();

		RuleFor(x => x.From)
			.NotEmpty()
			.WithMessage("The 'From' date is required.")
			.Must(from => from <= DateOnly.FromDateTime(DateTime.Now))
			.WithMessage("The 'From' date cannot be in the future.");

		RuleFor(x => x.PreviousMonths)
			.GreaterThan(0).WithMessage("PreviousMonths must be greater than 0.")
			.LessThanOrEqualTo(configurationService.SpendingMaxMonths)
			.WithMessage(x => $"PreviousMonths cannot exceed {configurationService.SpendingMaxMonths}.");
	}
}
