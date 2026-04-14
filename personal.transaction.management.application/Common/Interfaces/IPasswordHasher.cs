namespace personal.transaction.management.application.Common.Interfaces;

public interface IPasswordHasher
{
	string Hash(string password);
	bool Verify(string password, string hash);
}
