using FluentValidation;

namespace personal.transaction.management.application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
	public CreateAccountCommandValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
		RuleFor(x => x.AccountType).IsInEnum();
		RuleFor(x => x.Currency).NotEmpty().Length(3);
		RuleFor(x => x.Amount).GreaterThan(0);
	}
}
