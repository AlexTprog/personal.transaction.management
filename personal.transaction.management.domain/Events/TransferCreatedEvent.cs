using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.events;

public record TransferCreatedEvent(
	Guid TransferId,
	Guid SourceAccountId,
	Guid DestinationAccountId,
	Money SourceAmount,
	Money DestinationAmount,
	decimal? ExchangeRate) : IDomainEvent;
