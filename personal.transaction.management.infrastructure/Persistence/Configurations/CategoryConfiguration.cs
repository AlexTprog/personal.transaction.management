using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.ToTable("categories");

		builder.HasKey(c => c.Id);
		builder.Property(c => c.Id).HasColumnName("id");

		builder.Property(c => c.UserId).HasColumnName("user_id");  // nullable — null = system category

		builder.Property(c => c.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(c => c.Icon)
			.HasColumnName("icon")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(c => c.Color)
			.HasColumnName("color")
			.HasMaxLength(7)
			.IsRequired()
			.HasConversion(
				h => h.Value,
				v => HexColor.From(v));

		builder.Property(c => c.CategoryType)
			.HasColumnName("category_type")
			.IsRequired();

		builder.Property(c => c.IsSystem)
			.HasColumnName("is_system")
			.IsRequired();

		builder.Property(c => c.IsActive)
			.HasColumnName("is_active")
			.IsRequired();

		builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(256).IsRequired();
		builder.Property(c => c.ModifiedBy).HasColumnName("modified_by").HasMaxLength(256);
		builder.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(c => c.ModifiedAt).HasColumnName("modified_at");

		builder.HasIndex(c => new { c.UserId, c.Name }).HasDatabaseName("ix_categories_user_id_name");
		builder.HasIndex(c => c.IsSystem).HasDatabaseName("ix_categories_is_system");
	}
}
