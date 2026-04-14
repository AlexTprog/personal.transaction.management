namespace personal.transaction.management.application.Common.Exceptions;

public sealed class UnauthorizedException : Exception
{
	public UnauthorizedException(string message) : base(message) { }
}
