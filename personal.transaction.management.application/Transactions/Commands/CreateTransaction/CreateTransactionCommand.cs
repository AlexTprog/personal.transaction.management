using MediatR;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
	Guid AccountId,
	Guid UserId,
	Guid CategoryId,
	decimal Amount,
	string Currency,
	TransactionTypeEnum TransactionType,
	string? Description,
	DateOnly Date,
	decimal? ExchangeRate,
	string? AttachmentUrl,
	List<Guid>? TagIds) : IRequest<Guid>;
