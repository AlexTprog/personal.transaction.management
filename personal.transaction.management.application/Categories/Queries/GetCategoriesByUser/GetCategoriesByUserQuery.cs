using MediatR;
using personal.transaction.management.application.Categories.Dtos;

namespace personal.transaction.management.application.Categories.Queries.GetCategoriesByUser;

public record GetCategoriesByUserQuery(Guid UserId) : IRequest<IReadOnlyList<CategoryDto>>;
