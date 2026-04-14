using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.entities;

public class Tag
{
	public Guid Id { get; private set; }
	public Guid? UserId { get; private set; }   // null = system tag
	public string Name { get; private set; } = string.Empty;
	public bool IsSystem { get; private set; }

	// Required by EF Core
	private Tag() { }

	private Tag(Guid? userId, string name, bool isSystem)
	{
		Id = Guid.NewGuid();
		UserId = userId;
		Name = name;
		IsSystem = isSystem;
	}

	public static Tag CreateUserTag(Guid userId, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new DomainValidationException("Name", "Tag name cannot be empty.");

		return new Tag(userId, name.Trim(), false);
	}

	public static Tag CreateSystemTag(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new DomainValidationException("Name", "Tag name cannot be empty.");

		return new Tag(null, name.Trim(), true);
	}

	public void Rename(Guid requestingUserId, string name)
	{
		if (IsSystem)
			throw new SystemTagModificationException();

		if (UserId != requestingUserId)
			throw new UnauthorizedTagAccessException();

		if (string.IsNullOrWhiteSpace(name))
			throw new DomainValidationException("Name", "Tag name cannot be empty.");

		Name = name.Trim();
	}
}
