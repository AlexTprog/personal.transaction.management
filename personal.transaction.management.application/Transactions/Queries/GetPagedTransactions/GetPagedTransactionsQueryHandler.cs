using MediatR;
using personal.transaction.management.application.Common;
using personal.transaction.management.application.Transactions.Dtos;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Transactions.Queries.GetPagedTransactions;

public sealed class GetPagedTransactionsQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<GetPagedTransactionsQuery, PagedResult<TransactionDto>>
{
	public async Task<PagedResult<TransactionDto>> Handle(GetPagedTransactionsQuery request, CancellationToken cancellationToken)
	{
		var (items, totalCount) = await transactionRepository.GetPagedByUserAsync(
			request.UserId, request.Page, request.PageSize, cancellationToken);

		return new PagedResult<TransactionDto>(
			items.Select(TransactionDto.FromEntity).ToList(),
			request.Page,
			request.PageSize,
			totalCount);
	}
}
