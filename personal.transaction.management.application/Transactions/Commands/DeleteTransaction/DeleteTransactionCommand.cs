using MediatR;

namespace personal.transaction.management.application.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid TransactionId, Guid UserId) : IRequest;
