using FluentValidation;

namespace personal.transaction.management.application.Accounts.Commands.UpdateAccount;

public sealed class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
	public UpdateAccountCommandValidator()
	{
		RuleFor(x => x.AccountId).NotEmpty();
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
	}
}
