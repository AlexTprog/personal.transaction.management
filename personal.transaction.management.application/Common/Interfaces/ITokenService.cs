namespace personal.transaction.management.application.Common.Interfaces;

public interface ITokenService
{
	string GenerateToken(Guid userId, string email, string fullName);
}
