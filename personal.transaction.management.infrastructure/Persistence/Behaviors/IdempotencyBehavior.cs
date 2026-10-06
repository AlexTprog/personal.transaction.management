using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using personal.transaction.management.application.Abstractions;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.infrastructure.Persistence.Idempotency;

namespace personal.transaction.management.infrastructure.Persistence.Behaviors;

public sealed class IdempotencyBehavior<TRequest, TResponse>(ApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IIdempotentCommand { RequestId: var requestId } || requestId == Guid.Empty)
            return await next(cancellationToken);

        var name = typeof(TRequest).Name;
        var requestHash = ComputeHash(request);

        var existing = await FindAsync(requestId, cancellationToken);
        if (existing is not null)
            return Replay(existing, requestId, name, requestHash);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var record = IdempotentRequest.Create(requestId, name, requestHash);
        dbContext.IdempotentRequests.Add(record);

        try
        {
            // Claims the key. A concurrent duplicate blocks here on the primary key until the
            // first request commits, then fails with a unique violation and replays its response.
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            await transaction.RollbackAsync(cancellationToken);
            dbContext.Entry(record).State = EntityState.Detached;

            existing = await FindAsync(requestId, cancellationToken)
                ?? throw new ConflictException("The request is already being processed. Please try again.");

            return Replay(existing, requestId, name, requestHash);
        }

        // If the handler throws, the transaction is rolled back together with the claimed key,
        // so the client can safely retry with the same key.
        var response = await next(cancellationToken);

        record.Complete(JsonSerializer.Serialize(response));
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return response;
    }

    private Task<IdempotentRequest?> FindAsync(Guid requestId, CancellationToken cancellationToken) =>
        dbContext.IdempotentRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

    private static TResponse Replay(IdempotentRequest existing, Guid requestId, string name, string requestHash)
    {
        if (!existing.Matches(name, requestHash))
            throw new IdempotencyKeyReuseException(requestId);

        return JsonSerializer.Deserialize<TResponse>(existing.Response!)!;
    }

    private static string ComputeHash(TRequest request)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(request, request.GetType());
        return Convert.ToHexString(SHA256.HashData(json));
    }
}
