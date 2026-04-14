using personal.transaction.management.domain.events;

namespace personal.transaction.management.domain.abstractions;

public abstract class BaseAuditable
{
	private readonly List<IDomainEvent> _domainEvents = [];

	public string CreatedBy { get; private set; } = string.Empty;
	public string? ModifiedBy { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public DateTime? ModifiedAt { get; private set; }

	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	// Required by EF Core — do not use in application code
	protected BaseAuditable() { }

	protected BaseAuditable(string createdBy)
	{
		CreatedBy = createdBy;
		CreatedAt = DateTime.UtcNow;
	}

	protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
		_domainEvents.Add(domainEvent);

	public void ClearDomainEvents() => _domainEvents.Clear();

	protected void UpdateAuditInfo(string modifiedBy)
	{
		ModifiedBy = modifiedBy;
		ModifiedAt = DateTime.UtcNow;
	}
}
