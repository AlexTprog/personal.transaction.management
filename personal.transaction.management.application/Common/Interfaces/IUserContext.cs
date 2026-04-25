namespace personal.transaction.management.application.Common.Interfaces;

public interface IUserContextService
{
	Guid UserId { get; }
	string FullName { get; }
}