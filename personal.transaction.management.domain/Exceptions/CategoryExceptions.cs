namespace personal.transaction.management.domain.exceptions;

public sealed class SystemCategoryModificationException : DomainException
{
	public SystemCategoryModificationException()
		: base("System categories cannot be modified.") { }
}

public sealed class SystemCategoryDeactivationException : DomainException
{
	public SystemCategoryDeactivationException()
		: base("System categories cannot be deactivated.") { }
}
