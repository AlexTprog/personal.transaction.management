using MediatR;

namespace personal.transaction.management.application.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
	Guid TransactionId,
	Guid UserId,
	Guid CategoryId,
	decimal Amount,
	string Currency,
	string? Description,
	DateOnly Date,
	string? AttachmentUrl) : IRequest;
