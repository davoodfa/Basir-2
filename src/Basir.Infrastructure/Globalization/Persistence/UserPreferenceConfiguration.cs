using Basir.Domain.Entities.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basir.Infrastructure.Globalization.Persistence;

public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.ToTable("UserPreferences", "Globalization");

        builder.HasKey(up => up.Id);

        builder.Property(up => up.TimeZoneId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(up => up.CalendarType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(up => up.NumberFormat)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(up => up.Language)
            .WithMany()
            .HasForeignKey(up => up.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(up => up.Theme)
            .WithMany()
            .HasForeignKey(up => up.ThemeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(up => up.UserId)
            .HasDatabaseName("IX_UserPreferences_UserId")
            .IsUnique();
    }
}
