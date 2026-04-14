using FluentValidation;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
	public CreateTransactionCommandValidator()
	{
		RuleFor(x => x.AccountId).NotEmpty();
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
		RuleFor(x => x.Amount).GreaterThan(0);
		RuleFor(x => x.Currency).NotEmpty().Length(3);
		RuleFor(x => x.TransactionType).IsInEnum();
		RuleFor(x => x.Date).NotEmpty();
		RuleFor(x => x.AttachmentUrl).MaximumLength(2048).When(x => x.AttachmentUrl is not null);

		RuleFor(x => x.TransactionType)
			.NotEqual(TransactionTypeEnum.TransferIn)
			.NotEqual(TransactionTypeEnum.TransferOut)
			.WithMessage("Use the CreateTransfer endpoint for transfer transactions.");
	}
}
