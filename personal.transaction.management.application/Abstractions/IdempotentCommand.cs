using MediatR;

namespace personal.transaction.management.application.Abstractions;

/// <summary>
/// Marks a command whose execution must happen at most once per <see cref="RequestId"/>.
/// A <see cref="Guid.Empty"/> request id opts out of idempotency (the command always runs).
/// </summary>
public interface IIdempotentCommand
{
    Guid RequestId { get; }
}

public abstract record IdempotentCommand(Guid RequestId) : IRequest, IIdempotentCommand;

public abstract record IdempotentCommand<TResponse>(Guid RequestId) : IRequest<TResponse>, IIdempotentCommand;
