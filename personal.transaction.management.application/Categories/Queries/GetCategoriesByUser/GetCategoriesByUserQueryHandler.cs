using MediatR;
using personal.transaction.management.application.Categories.Dtos;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Categories.Queries.GetCategoriesByUser;

public sealed class GetCategoriesByUserQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesByUserQuery, IReadOnlyList<CategoryDto>>
{
	public async Task<IReadOnlyList<CategoryDto>> Handle(
		GetCategoriesByUserQuery request, CancellationToken cancellationToken)
	{
		var categories = await categoryRepository.GetByUserIdAsync(request.UserId, cancellationToken);
		return [.. categories.Select(CategoryDto.FromEntity)];
	}
}
