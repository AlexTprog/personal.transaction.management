using MediatR;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Guid>
{
	public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = Category.CreateUserCategory(
			request.UserId,
			request.Name,
			request.Icon,
			request.Color,
			request.CategoryType);

		await categoryRepository.AddAsync(category, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return category.Id;
	}
}
