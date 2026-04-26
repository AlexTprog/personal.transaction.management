using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
	public void Configure(EntityTypeBuilder<Transaction> builder)
	{
		builder.ToTable("transactions");

		builder.HasKey(t => t.Id);
		builder.Property(t => t.Id).HasColumnName("id");

		builder.Property(t => t.AccountId).HasColumnName("account_id").IsRequired();
		builder.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
		builder.Property(t => t.CategoryId).HasColumnName("category_id").IsRequired();

		builder.ComplexProperty(t => t.Amount, money =>
		{
			money.Property(m => m.Value)
				.HasColumnName("amount")
				.HasColumnType("decimal(18,4)")
				.IsRequired();

			money.Property(m => m.Currency)
				.HasColumnName("currency")
				.HasMaxLength(3)
				.IsRequired()
				.HasConversion(
					c => c.Code,
					code => Currency.From(code));
		});

		builder.Property(t => t.TransactionType)
			.HasColumnName("transaction_type")
			.IsRequired();

		builder.Property(t => t.Description)
			.HasColumnName("description")
			.HasMaxLength(500);

		builder.Property(t => t.Date)
			.HasColumnName("date")
			.IsRequired();

		builder.Property(t => t.TransferId)
			.HasColumnName("transfer_id");

		builder.Property(t => t.ExchangeRate)
			.HasColumnName("exchange_rate")
			.HasColumnType("decimal(18,6)");

		builder.Property(t => t.AttachmentUrl)
			.HasColumnName("attachment_url")
			.HasMaxLength(2048);

		builder.Property(t => t.CreatedBy).HasColumnName("created_by").HasMaxLength(256).IsRequired();
		builder.Property(t => t.ModifiedBy).HasColumnName("modified_by").HasMaxLength(256);
		builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(t => t.ModifiedAt).HasColumnName("modified_at");

		builder.HasMany(t => t.Tags)
			.WithOne(tt => tt.Transaction)
			.HasForeignKey(tt => tt.TransactionId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Navigation(t => t.Tags).HasField("_tags");

		builder.HasOne<Account>()
			.WithMany()
			.HasForeignKey(t => t.AccountId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne<User>()
			.WithMany()
			.HasForeignKey(t => t.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne<Category>()
			.WithMany()
			.HasForeignKey(t => t.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(t => t.UserId).HasDatabaseName("ix_transactions_user_id");
		builder.HasIndex(t => t.AccountId).HasDatabaseName("ix_transactions_account_id");
		builder.HasIndex(t => t.TransferId).HasDatabaseName("ix_transactions_transfer_id");
		builder.HasIndex(t => t.Date).HasDatabaseName("ix_transactions_date");
	}
}
