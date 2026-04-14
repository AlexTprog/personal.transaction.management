using MediatR;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransfer;

public record CreateTransferCommand(
	Guid UserId,
	Guid SourceAccountId,
	Guid DestinationAccountId,
	Guid CategoryId,
	decimal Amount,
	string? Description,
	DateOnly Date,
	decimal? ExchangeRate) : IRequest<Guid>;
