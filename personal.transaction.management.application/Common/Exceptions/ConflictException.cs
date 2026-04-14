namespace personal.transaction.management.application.Common.Exceptions;

public class ConflictException : Exception
{
	public ConflictException(string message) : base(message) { }
}
