using MediatR;
using personal.transaction.management.application.Common;
using personal.transaction.management.application.Transactions.Dtos;

namespace personal.transaction.management.application.Transactions.Queries.GetPagedTransactions;

public record GetPagedTransactionsQuery(Guid UserId, int Page, int PageSize)
	: IRequest<PagedResult<TransactionDto>>;
