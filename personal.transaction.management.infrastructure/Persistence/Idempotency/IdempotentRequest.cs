namespace personal.transaction.management.infrastructure.Persistence.Idempotency;

public sealed class IdempotentRequest
{
    public Guid Id { get; private init; }
    public string Name { get; private init; } = string.Empty;
    public string RequestHash { get; private init; } = string.Empty;
    public string? Response { get; private set; }
    public DateTime CreatedOnUtc { get; private init; }

    private IdempotentRequest() { }

    public static IdempotentRequest Create(Guid id, string name, string requestHash)
    {
        return new IdempotentRequest
        {
            Id = id,
            Name = name,
            RequestHash = requestHash,
            CreatedOnUtc = DateTime.UtcNow
        };
    }

    public bool Matches(string name, string requestHash) =>
        Name == name && RequestHash == requestHash;

    public void Complete(string response)
    {
        Response = response;
    }
}
