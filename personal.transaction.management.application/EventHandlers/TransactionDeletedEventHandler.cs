using MediatR;
using personal.transaction.management.application.Common;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.EventHandlers;

public sealed class TransactionDeletedEventHandler(IAccountRepository accountRepository) : INotificationHandler<DomainEventNotification<TransactionDeletedEvent>>
{
	public async Task Handle(DomainEventNotification<TransactionDeletedEvent> notification, CancellationToken cancellationToken)
	{
		var @event = notification.Event;

		var account = await accountRepository.GetByIdAsync(@event.AccountId, cancellationToken)
			?? throw new InvalidOperationException(
				$"Account '{@event.AccountId}' not found while processing TransactionDeletedEvent.");

		// Reverse the effect of the deleted transaction
		if (@event.TransactionType is TransactionTypeEnum.Income or TransactionTypeEnum.TransferIn)
			account.Debit(@event.Amount);
		else
			account.Credit(@event.Amount);
	}
}
