using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder.ToTable("accounts");

		builder.HasKey(a => a.Id);
		builder.Property(a => a.Id).HasColumnName("id");

		builder.Property(a => a.UserId).HasColumnName("user_id").IsRequired();

		builder.Property(a => a.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(a => a.AccountType)
			.HasColumnName("account_type")
			.IsRequired();

		builder.Property(a => a.Currency)
			.HasColumnName("currency")
			.HasMaxLength(3)
			.IsRequired()
			.HasConversion(
				c => c.Code,
				code => Currency.From(code));

		builder.Property(a => a.Balance)
			.HasColumnName("balance")
			.HasColumnType("decimal(18,4)")
			.IsRequired();

		builder.Property(a => a.IsActive)
			.HasColumnName("is_active")
			.IsRequired();

		builder.Property(a => a.CreatedBy).HasColumnName("created_by").HasMaxLength(256).IsRequired();
		builder.Property(a => a.ModifiedBy).HasColumnName("modified_by").HasMaxLength(256);
		builder.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(a => a.ModifiedAt).HasColumnName("modified_at");

		builder.HasOne<User>()
			.WithMany(u => u.Accounts)
			.HasForeignKey(a => a.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(a => a.UserId).HasDatabaseName("ix_accounts_user_id");
		builder.Property(e => e.Xmin).IsRowVersion();
	}
}
