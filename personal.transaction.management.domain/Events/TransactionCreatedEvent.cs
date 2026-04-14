using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.events;

public record TransactionCreatedEvent(
	Guid TransactionId,
	Guid AccountId,
	Guid UserId,
	Money Amount,
	TransactionTypeEnum TransactionType) : IDomainEvent;
