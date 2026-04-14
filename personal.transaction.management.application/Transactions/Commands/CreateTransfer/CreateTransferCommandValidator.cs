using FluentValidation;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransfer;

public sealed class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
{
	public CreateTransferCommandValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.SourceAccountId).NotEmpty();
		RuleFor(x => x.DestinationAccountId)
			.NotEmpty()
			.NotEqual(x => x.SourceAccountId)
			.WithMessage("Source and destination accounts must be different.");
		RuleFor(x => x.CategoryId).NotEmpty();
		RuleFor(x => x.Amount).GreaterThan(0);
		RuleFor(x => x.Date).NotEmpty();
		RuleFor(x => x.ExchangeRate)
			.GreaterThan(0)
			.When(x => x.ExchangeRate.HasValue);
	}
}
