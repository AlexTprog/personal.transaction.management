using personal.transaction.management.domain.abstractions;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.entities;

public class User : BaseAuditable
{
	public Guid Id { get; private set; }
	public Email Email { get; private set; } = null!;
	public string FullName { get; private set; } = string.Empty;
	public string PasswordHash { get; private set; } = string.Empty;
	public bool IsActive { get; private set; }

	private readonly List<Account> _accounts = [];
	public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();

	private User() { }

	private User(Email email, string fullName, string passwordHash)
	{
		Id = Guid.NewGuid();
		Email = email;
		FullName = fullName;
		PasswordHash = passwordHash;
		IsActive = true;
	}

	public static User Create(string email, string fullName, string passwordHash)
	{
		if (string.IsNullOrWhiteSpace(fullName))
			throw new DomainValidationException("FullName", "Full name cannot be empty.");

		if (string.IsNullOrWhiteSpace(passwordHash))
			throw new DomainValidationException("PasswordHash", "Password hash cannot be empty.");

		return new User(Email.From(email), fullName.Trim(), passwordHash);
	}

	public void UpdateProfile(string fullName)
	{
		if (string.IsNullOrWhiteSpace(fullName))
			throw new DomainValidationException("FullName", "Full name cannot be empty.");

		FullName = fullName.Trim();
	}

	public void ChangePassword(string newPasswordHash)
	{
		if (string.IsNullOrWhiteSpace(newPasswordHash))
			throw new DomainValidationException("PasswordHash", "Password hash cannot be empty.");

		PasswordHash = newPasswordHash;
	}

	public void Deactivate()
	{
		IsActive = false;
	}
}
