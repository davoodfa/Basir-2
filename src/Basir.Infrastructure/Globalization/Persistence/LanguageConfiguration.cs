using Basir.Domain.Entities.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basir.Infrastructure.Globalization.Persistence;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages", "Globalization");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(l => l.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.NativeName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.Direction)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(l => l.FlagIcon)
            .HasMaxLength(50);

        builder.Property(l => l.CreatedBy)
            .HasMaxLength(256);

        builder.Property(l => l.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(l => l.Code)
            .HasDatabaseName("IX_Languages_Code")
            .IsUnique();
    }
}
