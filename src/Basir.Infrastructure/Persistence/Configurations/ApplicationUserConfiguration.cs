using Basir.Domain.Entities.Identity;
using Basir.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basir.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", "Identity");

        builder.Property(u => u.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Status)
            .HasConversion<int>()
            .HasDefaultValue(Domain.Enums.UserStatus.Pending);

        builder.Property(u => u.LastLoginAt);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(256);

        builder.Property(u => u.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("IX_User_NormalizedEmail")
            .IsUnique();

        builder.HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("IX_User_NormalizedUserName")
            .IsUnique();

        builder.HasIndex(u => u.SecurityStamp)
            .HasDatabaseName("IX_User_SecurityStamp");

        builder.Ignore(u => u.DomainEvents);
        builder.Ignore(u => u.FullName);
    }
}
