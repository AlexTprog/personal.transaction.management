using MediatR;
using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.abstractions;

namespace personal.transaction.management.infrastructure.Persistence.Behaviors;

public sealed class ConcurrencyRetryBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int MaxRetries = 2;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (attempt >= MaxRetries)
                    throw new ConflictException("The resource was modified by another request. Please try again.");

                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is BaseAuditable entity)
                        entity.ClearDomainEvents();

                    await entry.ReloadAsync(cancellationToken);
                }
            }
        }
    }
}
