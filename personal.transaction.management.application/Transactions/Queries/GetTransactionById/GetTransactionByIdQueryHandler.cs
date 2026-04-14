using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.application.Transactions.Dtos;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Transactions.Queries.GetTransactionById;

public sealed class GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
	public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
	{
		var transaction = await transactionRepository.GetByIdAndUserIdAsync(
			request.TransactionId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

		return TransactionDto.FromEntity(transaction);
	}
}
