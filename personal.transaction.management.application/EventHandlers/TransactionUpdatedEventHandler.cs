using MediatR;
using personal.transaction.management.application.Common;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.EventHandlers;

public sealed class TransactionUpdatedEventHandler(IAccountRepository accountRepository) : INotificationHandler<DomainEventNotification<TransactionUpdatedEvent>>
{
	public async Task Handle(DomainEventNotification<TransactionUpdatedEvent> notification, CancellationToken cancellationToken)
	{
		var @event = notification.Event;

		var account = await accountRepository.GetByIdAsync(@event.AccountId, cancellationToken)
			?? throw new InvalidOperationException(
				$"Account '{@event.AccountId}' not found while processing TransactionUpdatedEvent.");

		// Step 1: reverse the previous amount
		if (@event.TransactionType is TransactionTypeEnum.Income or TransactionTypeEnum.TransferIn)
			account.Debit(@event.PreviousAmount, "system");
		else
			account.Credit(@event.PreviousAmount, "system");

		// Step 2: apply the new amount (type cannot change on update)
		if (@event.TransactionType is TransactionTypeEnum.Income or TransactionTypeEnum.TransferIn)
			account.Credit(@event.NewAmount, "system");
		else
			account.Debit(@event.NewAmount, "system");
	}
}
