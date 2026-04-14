using MediatR;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
	Guid UserId,
	string Name,
	string Icon,
	string Color,
	CategoryTypeEnum CategoryType) : IRequest<Guid>;
