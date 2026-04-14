using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Categories.Dtos;

public record CategoryDto
{
	public Guid Id { get; init; }
	public Guid? UserId { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Icon { get; init; } = string.Empty;
	public string Color { get; init; } = string.Empty;
	public CategoryTypeEnum CategoryType { get; init; }
	public bool IsSystem { get; init; }
	public bool IsActive { get; init; }

	public static CategoryDto FromEntity(Category c) => new()
	{
		Id = c.Id,
		UserId = c.UserId,
		Name = c.Name,
		Icon = c.Icon,
		Color = c.Color.Value,
		CategoryType = c.CategoryType,
		IsSystem = c.IsSystem,
		IsActive = c.IsActive
	};
}
