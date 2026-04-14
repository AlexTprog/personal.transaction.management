using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
	{
		builder.ToTable("tags");

		builder.HasKey(t => t.Id);
		builder.Property(t => t.Id).HasColumnName("id");

		builder.Property(t => t.UserId).HasColumnName("user_id");  // nullable — null = system tag

		builder.Property(t => t.Name)
			.HasColumnName("name")
			.HasMaxLength(50)
			.IsRequired();

		builder.Property(t => t.IsSystem)
			.HasColumnName("is_system")
			.IsRequired();

		builder.HasIndex(t => new { t.UserId, t.Name }).HasDatabaseName("ix_tags_user_id_name");
		builder.HasIndex(t => t.IsSystem).HasDatabaseName("ix_tags_is_system");
	}
}
