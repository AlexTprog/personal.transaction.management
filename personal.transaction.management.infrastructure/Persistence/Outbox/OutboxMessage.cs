using System.Text.Json;
using personal.transaction.management.domain.events;
using personal.transaction.management.infrastructure.Persistence.Outbox.Serialization;

namespace personal.transaction.management.infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private init; }
    public string Type { get; private init; } = string.Empty;
    public string Content { get; private init; } = string.Empty;
    public DateTime OccurredOnUtc { get; private init; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }
    public int Attempts { get; private set; }

    private OutboxMessage() { }

    public static OutboxMessage FromDomainEvent(IDomainEvent domainEvent)
    {
        var eventType = domainEvent.GetType();

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = eventType.FullName!,
            Content = JsonSerializer.Serialize(domainEvent, eventType, OutboxJsonSerializerOptions.Instance),
            OccurredOnUtc = DateTime.UtcNow
        };
    }

    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
    }

    public void MarkFailed(string error)
    {
        Attempts++;
        Error = error;
    }
}
