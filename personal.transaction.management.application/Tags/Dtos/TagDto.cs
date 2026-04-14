using personal.transaction.management.domain.entities;

namespace personal.transaction.management.application.Tags.Dtos;

public record TagDto
{
	public Guid Id { get; init; }
	public Guid? UserId { get; init; }
	public string Name { get; init; } = string.Empty;
	public bool IsSystem { get; init; }

	public static TagDto FromEntity(Tag t) => new()
	{
		Id = t.Id,
		UserId = t.UserId,
		Name = t.Name,
		IsSystem = t.IsSystem
	};
}
