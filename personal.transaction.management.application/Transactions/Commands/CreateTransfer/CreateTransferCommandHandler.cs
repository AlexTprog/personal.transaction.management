using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.repositories;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransfer;

public sealed class CreateTransferCommandHandler(
	ITransactionRepository transactionRepository,
	IAccountRepository accountRepository,
	ICategoryRepository categoryRepository,
	IUnitOfWork unitOfWork) : IRequestHandler<CreateTransferCommand, Guid>
{
	public async Task<Guid> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
	{
		var sourceAccount = await accountRepository.GetByIdAndUserIdAsync(
			request.SourceAccountId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Account), request.SourceAccountId);

		var destinationAccount = await accountRepository.GetByIdAndUserIdAsync(
			request.DestinationAccountId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Account), request.DestinationAccountId);

		var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
			?? throw new NotFoundException(nameof(Category), request.CategoryId);

		bool differentCurrencies = sourceAccount.Currency.Code != destinationAccount.Currency.Code;

		if (differentCurrencies && request.ExchangeRate is null)
			throw new DomainValidationException(
				"ExchangeRate",
				"Exchange rate is required when transferring between accounts with different currencies.");

		var transferId = Guid.NewGuid();
		var createdBy = request.UserId.ToString();

		var sourceAmount = Money.Of(request.Amount, sourceAccount.Currency);
		var destinationAmount = differentCurrencies
			? Money.Of(Math.Round(request.Amount * request.ExchangeRate!.Value, 4), destinationAccount.Currency)
			: sourceAmount;

		var transferOut = Transaction.Create(
			sourceAccount.Id, request.UserId, category.Id,
			sourceAmount, TransactionTypeEnum.TransferOut,
			request.Description, request.Date,
			transferId, request.ExchangeRate, null, createdBy);

		var transferIn = Transaction.Create(
			destinationAccount.Id, request.UserId, category.Id,
			destinationAmount, TransactionTypeEnum.TransferIn,
			request.Description, request.Date,
			transferId, request.ExchangeRate, null, createdBy);

		await transactionRepository.AddRangeAsync([transferOut, transferIn], cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return transferId;
	}
}
