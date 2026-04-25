using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Categories.Commands.DeactivateCategory;

public sealed class DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeactivateCategoryCommand>
{
	public async Task Handle(DeactivateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
			?? throw new NotFoundException(nameof(Category), request.CategoryId);

		if (await categoryRepository.HasTransactionsAsync(request.CategoryId, cancellationToken))
			throw new ConflictException("Cannot deactivate a category that has associated transactions.");

		category.Deactivate();

		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
