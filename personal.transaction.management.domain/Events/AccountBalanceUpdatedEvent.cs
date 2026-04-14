namespace personal.transaction.management.domain.events;

public record AccountBalanceUpdatedEvent(
	Guid AccountId,
	Guid UserId,
	decimal PreviousBalance,
	decimal NewBalance) : IDomainEvent;
