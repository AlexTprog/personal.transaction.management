using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand>
{
	public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
			?? throw new NotFoundException(nameof(Category), request.CategoryId);

		// Domain enforces system category guard
		category.Update(request.Name, request.Icon, request.Color, request.CategoryType,
			request.UserId.ToString());

		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
