using FluentValidation;

namespace personal.transaction.management.application.Transactions.Commands.UpdateTransaction;

public sealed class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
	public UpdateTransactionCommandValidator()
	{
		RuleFor(x => x.TransactionId).NotEmpty();
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
		RuleFor(x => x.Amount).GreaterThan(0);
		RuleFor(x => x.Currency).NotEmpty().Length(3);
		RuleFor(x => x.Date).NotEmpty();
		RuleFor(x => x.AttachmentUrl).MaximumLength(2048).When(x => x.AttachmentUrl is not null);
	}
}
