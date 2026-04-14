using MediatR;
using personal.transaction.management.application.Transactions.Dtos;

namespace personal.transaction.management.application.Transactions.Queries.GetTransactionById;

public record GetTransactionByIdQuery(Guid TransactionId, Guid UserId) : IRequest<TransactionDto>;
