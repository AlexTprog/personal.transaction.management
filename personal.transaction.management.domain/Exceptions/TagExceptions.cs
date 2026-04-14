namespace personal.transaction.management.domain.exceptions;

public sealed class SystemTagModificationException : DomainException
{
	public SystemTagModificationException()
		: base("System tags cannot be modified.") { }
}

public sealed class UnauthorizedTagAccessException : DomainException
{
	public UnauthorizedTagAccessException()
		: base("Cannot modify a tag that belongs to another user.") { }
}
