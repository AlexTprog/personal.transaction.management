namespace personal.transaction.management.domain.exceptions;

public sealed class DomainValidationException : DomainException
{
	public string Field { get; }

	public DomainValidationException(string field, string message) : base(message)
	{
		Field = field;
	}
}
