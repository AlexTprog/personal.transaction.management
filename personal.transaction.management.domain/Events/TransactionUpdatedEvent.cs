using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.events;

public record TransactionUpdatedEvent(
	Guid TransactionId,
	Guid AccountId,
	Money PreviousAmount,
	Money NewAmount,
	TransactionTypeEnum TransactionType) : IDomainEvent;
