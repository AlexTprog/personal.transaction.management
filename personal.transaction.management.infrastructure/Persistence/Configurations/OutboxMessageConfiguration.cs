using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.infrastructure.Persistence.Outbox;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.Type)
            .HasColumnName("type")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(m => m.Content)
            .HasColumnName("content")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(m => m.OccurredOnUtc)
            .HasColumnName("occurred_on_utc")
            .IsRequired();

        builder.Property(m => m.ProcessedOnUtc)
            .HasColumnName("processed_on_utc");

        builder.Property(m => m.Error)
            .HasColumnName("error")
            .HasColumnType("text");

        builder.Property(m => m.Attempts)
            .HasColumnName("attempts")
            .IsRequired();

        builder.HasIndex(m => m.ProcessedOnUtc)
            .HasDatabaseName("ix_outbox_messages_processed_on_utc");
    }
}
