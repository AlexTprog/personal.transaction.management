using MediatR;
using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Common;
using personal.transaction.management.application.Common.Interfaces;
using personal.transaction.management.domain.abstractions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.repositories;
using System.Reflection;

namespace personal.transaction.management.infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher, IUserContextService userContextService) : DbContext(options), IUnitOfWork
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Account> Accounts => Set<Account>();
	public DbSet<Transaction> Transactions => Set<Transaction>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Tag> Tags => Set<Tag>();
	public DbSet<TransactionTag> TransactionTags => Set<TransactionTag>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await SetAuditedFields();
		await DispatchDomainEventsAsync(cancellationToken);
		return await base.SaveChangesAsync(cancellationToken);
	}

	private async Task SetAuditedFields()
	{
		var entries = ChangeTracker
			.Entries<BaseAuditable>()
			.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

		foreach (var entry in entries)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Property(nameof(BaseAuditable.CreatedBy)).CurrentValue = userContextService.FullName;
					entry.Property(nameof(BaseAuditable.CreatedAt)).CurrentValue = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Property(nameof(BaseAuditable.ModifiedBy)).CurrentValue = userContextService.FullName;
					entry.Property(nameof(BaseAuditable.ModifiedAt)).CurrentValue = DateTime.UtcNow;
					break;
				default:
					break;
			}
		}
	}

	private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
	{
		List<IDomainEvent> pendingEvents;

		do
		{
			var entitiesWithEvents = ChangeTracker
				.Entries<BaseAuditable>()
				.Where(e => e.Entity.DomainEvents.Any())
				.Select(e => e.Entity)
				.ToList();

			pendingEvents = [.. entitiesWithEvents.SelectMany(e => e.DomainEvents)];

			entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

			foreach (var domainEvent in pendingEvents)
			{
				var notificationType = typeof(DomainEventNotification<>)
					.MakeGenericType(domainEvent.GetType());

				var notification = (INotification)Activator
					.CreateInstance(notificationType, domainEvent)!;

				await publisher.Publish(notification, cancellationToken);
			}
		} while (pendingEvents.Count > 0);
	}
}
