using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandHandler(
	ITransactionRepository transactionRepository,
	IAccountRepository accountRepository,
	ICategoryRepository categoryRepository,
	ITagRepository tagRepository,
	IUnitOfWork unitOfWork) : IRequestHandler<CreateTransactionCommand, Guid>
{
	public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
	{
		var account = await accountRepository.GetByIdAndUserIdAsync(
			request.AccountId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Account), request.AccountId);

		var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
			?? throw new NotFoundException(nameof(Category), request.CategoryId);

		var amount = Money.Of(request.Amount, request.Currency);

		var transaction = Transaction.Create(
			account.Id,
			request.UserId,
			category.Id,
			amount,
			request.TransactionType,
			request.Description,
			request.Date,
			transferId: null,
			request.ExchangeRate,
			request.AttachmentUrl,
			request.UserId.ToString());

		if (request.TagIds is { Count: > 0 })
		{
			var tags = await tagRepository.GetByUserIdAsync(request.UserId, cancellationToken);
			var tagMap = tags.ToDictionary(t => t.Id);

			foreach (var tagId in request.TagIds)
			{
				if (tagMap.TryGetValue(tagId, out var tag))
					transaction.AddTag(tag);
			}
		}

		await transactionRepository.AddAsync(transaction, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return transaction.Id;
	}
}
