using MediatR;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
	Guid CategoryId,
	Guid UserId,
	string Name,
	string Icon,
	string Color,
	CategoryTypeEnum CategoryType) : IRequest;
