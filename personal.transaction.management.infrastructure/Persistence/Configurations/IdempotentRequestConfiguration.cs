using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.infrastructure.Persistence.Idempotency;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.ToTable("idempotent_requests");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(r => r.RequestHash)
            .HasColumnName("request_hash")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(r => r.Response)
            .HasColumnName("response")
            .HasColumnType("jsonb");

        builder.Property(r => r.CreatedOnUtc)
            .HasColumnName("created_on_utc")
            .IsRequired();

        builder.HasIndex(r => r.CreatedOnUtc)
            .HasDatabaseName("ix_idempotent_requests_created_on_utc");
    }
}
