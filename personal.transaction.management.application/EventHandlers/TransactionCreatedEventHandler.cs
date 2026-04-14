using MediatR;
using personal.transaction.management.application.Common;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.EventHandlers;

public sealed class TransactionCreatedEventHandler(IAccountRepository accountRepository) : INotificationHandler<DomainEventNotification<TransactionCreatedEvent>>
{
	public async Task Handle(DomainEventNotification<TransactionCreatedEvent> notification, CancellationToken cancellationToken)
	{
		var @event = notification.Event;

		var account = await accountRepository.GetByIdAsync(@event.AccountId, cancellationToken)
			?? throw new InvalidOperationException(
				$"Account '{@event.AccountId}' not found while processing TransactionCreatedEvent.");

		// Income / TransferIn: money enters the account
		// Expense / TransferOut: money leaves the account
		if (@event.TransactionType is TransactionTypeEnum.Income or TransactionTypeEnum.TransferIn)
			account.Credit(@event.Amount, "system");
		else
			account.Debit(@event.Amount, "system");
	}
}
