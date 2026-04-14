using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Transactions.Commands.DeleteTransaction;

public sealed class DeleteTransactionCommandHandler(
	ITransactionRepository transactionRepository,
	IUnitOfWork unitOfWork) : IRequestHandler<DeleteTransactionCommand>
{
	public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
	{
		var transaction = await transactionRepository.GetByIdAndUserIdAsync(
			request.TransactionId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

		if (transaction.TransferId.HasValue)
		{
			// Delete both legs of the transfer atomically
			var legs = await transactionRepository.GetByTransferIdAsync(
				transaction.TransferId.Value, cancellationToken);

			foreach (var leg in legs)
			{
				leg.Delete();
				transactionRepository.Remove(leg);
			}
		}
		else
		{
			transaction.Delete();
			transactionRepository.Remove(transaction);
		}

		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
