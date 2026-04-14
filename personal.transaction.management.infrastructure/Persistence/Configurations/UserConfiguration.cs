using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("users");

		builder.HasKey(u => u.Id);
		builder.Property(u => u.Id).HasColumnName("id");

		builder.Property(u => u.FullName)
			.HasColumnName("full_name")
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(u => u.PasswordHash)
			.HasColumnName("password_hash")
			.IsRequired();

		builder.Property(u => u.IsActive)
			.HasColumnName("is_active")
			.IsRequired();

		builder.Property(u => u.Email)
			.HasColumnName("email")
			.HasMaxLength(256)
			.IsRequired()
			.HasConversion(
				e => e.Value,
				v => Email.From(v));

		builder.HasIndex(u => u.Email)
			.IsUnique()
			.HasDatabaseName("ix_users_email");

		builder.Property(u => u.CreatedBy).HasColumnName("created_by").HasMaxLength(256).IsRequired();
		builder.Property(u => u.ModifiedBy).HasColumnName("modified_by").HasMaxLength(256);
		builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(u => u.ModifiedAt).HasColumnName("modified_at");

		builder.Navigation(u => u.Accounts).HasField("_accounts");
	}
}
