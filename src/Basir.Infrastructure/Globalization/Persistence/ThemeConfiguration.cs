using Basir.Domain.Entities.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basir.Infrastructure.Globalization.Persistence;

public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
{
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder.ToTable("Themes", "Globalization");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.ThemeType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.PrimaryColor)
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(t => t.SecondaryColor)
            .HasMaxLength(7);

        builder.Property(t => t.CssVariables)
            .HasColumnType("nvarchar(max)");

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(256);

        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(t => t.Name)
            .HasDatabaseName("IX_Themes_Name")
            .IsUnique();
    }
}
