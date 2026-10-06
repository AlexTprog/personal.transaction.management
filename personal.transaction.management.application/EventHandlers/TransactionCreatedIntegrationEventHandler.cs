using MediatR;
using Microsoft.Extensions.Logging;
using personal.transaction.management.application.Common;
using personal.transaction.management.domain.events;

namespace personal.transaction.management.application.EventHandlers;

/// <summary>
/// Dispatched asynchronously by the outbox worker, outside the original request's transaction.
/// Stands in for downstream integrations (push notifications, analytics, webhooks) that must
/// never be called synchronously inside the request that writes the transaction.
/// </summary>
public sealed class TransactionCreatedIntegrationEventHandler(ILogger<TransactionCreatedIntegrationEventHandler> logger)
    : INotificationHandler<IntegrationEventNotification<TransactionCreatedEvent>>
{
    public Task Handle(IntegrationEventNotification<TransactionCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var @event = notification.Event;

        logger.LogInformation(
            "Integration event dispatched: transaction {TransactionId} ({TransactionType}, {Amount}) created on account {AccountId}.",
            @event.TransactionId, @event.TransactionType, @event.Amount, @event.AccountId);

        return Task.CompletedTask;
    }
}
