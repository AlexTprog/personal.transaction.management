using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal.transaction.management.domain.entities;

namespace personal.transaction.management.infrastructure.Persistence.Configurations;

internal sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("budgets");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.Property(b => b.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(b => b.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(b => b.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(b => b.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(b => b.ModifiedBy)
            .HasColumnName("modified_by")
            .HasMaxLength(256);
        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        builder.Property(b => b.ModifiedAt)
            .HasColumnName("modified_at");

        builder.HasIndex(b => b.UserId)
            .HasDatabaseName("ix_budgets_user_id");
    }
}
