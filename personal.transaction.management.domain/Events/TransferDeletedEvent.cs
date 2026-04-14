using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.events;

public record TransferDeletedEvent(
	Guid TransferId,
	Guid SourceAccountId,
	Guid DestinationAccountId,
	Money SourceAmount) : IDomainEvent;
