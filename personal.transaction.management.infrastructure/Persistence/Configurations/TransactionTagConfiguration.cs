using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class TransactionTagConfiguration : IEntityTypeConfiguration<TransactionTag>
{
	public void Configure(EntityTypeBuilder<TransactionTag> builder)
	{
		builder.ToTable("transaction_tags");

		builder.HasKey(tt => new { tt.TransactionId, tt.TagId });

		builder.Property(tt => tt.TransactionId).HasColumnName("transaction_id");
		builder.Property(tt => tt.TagId).HasColumnName("tag_id");

		builder.HasOne(tt => tt.Tag)
			.WithMany()
			.HasForeignKey(tt => tt.TagId)
			.OnDelete(DeleteBehavior.Restrict);

		// Transaction → TransactionTag cascade is configured in TransactionConfiguration
	}
}
