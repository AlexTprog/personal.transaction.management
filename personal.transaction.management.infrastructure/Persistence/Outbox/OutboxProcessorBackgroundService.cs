using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using personal.transaction.management.application.Common;
using personal.transaction.management.application.Common.Diagnostics;
using personal.transaction.management.domain.events;
using personal.transaction.management.infrastructure.Configuration;
using personal.transaction.management.infrastructure.Persistence.Outbox.Serialization;

namespace personal.transaction.management.infrastructure.Persistence.Outbox;

public sealed class OutboxProcessorBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxOptions> options,
    ILogger<OutboxProcessorBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = TimeSpan.FromSeconds(Math.Max(1, options.Value.PollingIntervalSeconds));
        using var timer = new PeriodicTimer(interval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error while processing the outbox.");
            }
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        var maxAttempts = options.Value.MaxAttempts;

        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null && m.Attempts < maxAttempts)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(options.Value.BatchSize)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
            return;

        foreach (var message in messages)
            await DispatchAsync(message, publisher, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchAsync(OutboxMessage message, IPublisher publisher, CancellationToken cancellationToken)
    {
        using var activity = ApplicationDiagnostics.ActivitySource.StartActivity(
            $"Outbox {message.Type}",
            ActivityKind.Internal);

        activity?.SetTag("outbox.message_id", message.Id);
        activity?.SetTag("outbox.event_type", message.Type);

        try
        {
            var eventType = typeof(IDomainEvent).Assembly.GetType(message.Type)
                ?? throw new InvalidOperationException($"Could not resolve domain event type '{message.Type}'.");

            var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(
                message.Content, eventType, OutboxJsonSerializerOptions.Instance)!;

            var notificationType = typeof(IntegrationEventNotification<>).MakeGenericType(eventType);
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;

            await publisher.Publish(notification, cancellationToken);

            message.MarkProcessed();
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to dispatch outbox message {MessageId} of type {MessageType}", message.Id, message.Type);
            message.MarkFailed(ex.Message);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
        }
    }
}
