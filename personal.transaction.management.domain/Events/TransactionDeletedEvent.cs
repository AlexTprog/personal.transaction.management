using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.events;

public record TransactionDeletedEvent(
	Guid TransactionId,
	Guid AccountId,
	Money Amount,
	TransactionTypeEnum TransactionType) : IDomainEvent;
