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

	protected BaseAuditable() { }

	protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
		_domainEvents.Add(domainEvent);

	public void ClearDomainEvents() => _domainEvents.Clear();

}
