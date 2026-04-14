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

	// Required by EF Core
	private User() { }

	private User(Email email, string fullName, string passwordHash, string createdBy)
		: base(createdBy)
	{
		Id = Guid.NewGuid();
		Email = email;
		FullName = fullName;
		PasswordHash = passwordHash;
		IsActive = true;
	}

	public static User Create(string email, string fullName, string passwordHash, string createdBy)
	{
		if (string.IsNullOrWhiteSpace(fullName))
			throw new DomainValidationException("FullName", "Full name cannot be empty.");

		if (string.IsNullOrWhiteSpace(passwordHash))
			throw new DomainValidationException("PasswordHash", "Password hash cannot be empty.");

		return new User(Email.From(email), fullName.Trim(), passwordHash, createdBy);
	}

	public void UpdateProfile(string fullName, string modifiedBy)
	{
		if (string.IsNullOrWhiteSpace(fullName))
			throw new DomainValidationException("FullName", "Full name cannot be empty.");

		FullName = fullName.Trim();
		UpdateAuditInfo(modifiedBy);
	}

	public void ChangePassword(string newPasswordHash, string modifiedBy)
	{
		if (string.IsNullOrWhiteSpace(newPasswordHash))
			throw new DomainValidationException("PasswordHash", "Password hash cannot be empty.");

		PasswordHash = newPasswordHash;
		UpdateAuditInfo(modifiedBy);
	}

	public void Deactivate(string modifiedBy)
	{
		IsActive = false;
		UpdateAuditInfo(modifiedBy);
	}
}
