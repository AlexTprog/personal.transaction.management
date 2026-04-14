using MediatR;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = Category.CreateUserCategory(
			request.UserId,
			request.Name,
			request.Icon,
			request.Color,
			request.CategoryType,
			request.UserId.ToString());

		await _categoryRepository.AddAsync(category, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return category.Id;
	}
}
