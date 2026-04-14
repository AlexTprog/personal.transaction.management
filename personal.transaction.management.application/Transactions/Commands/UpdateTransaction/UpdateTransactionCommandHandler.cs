using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.application.Transactions.Commands.UpdateTransaction;

public sealed class UpdateTransactionCommandHandler(
	ITransactionRepository transactionRepository,
	ICategoryRepository categoryRepository,
	IUnitOfWork unitOfWork) : IRequestHandler<UpdateTransactionCommand>
{
	public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
	{
		var transaction = await transactionRepository.GetByIdAndUserIdAsync(
			request.TransactionId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

		var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
			?? throw new NotFoundException(nameof(Category), request.CategoryId);

		var newAmount = Money.Of(request.Amount, request.Currency);

		transaction.Update(
			category.Id,
			newAmount,
			request.Description,
			request.Date,
			request.AttachmentUrl,
			request.UserId.ToString());

		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
