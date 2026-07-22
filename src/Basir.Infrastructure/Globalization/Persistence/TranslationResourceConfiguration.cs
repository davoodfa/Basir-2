using Basir.Domain.Entities.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basir.Infrastructure.Globalization.Persistence;

public class TranslationResourceConfiguration : IEntityTypeConfiguration<TranslationResource>
{
    public void Configure(EntityTypeBuilder<TranslationResource> builder)
    {
        builder.ToTable("Translations", "Globalization");

        builder.HasKey(tr => tr.Id);

        builder.Property(tr => tr.Module)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(tr => tr.ResourceKey)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(tr => tr.Value)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(tr => tr.CreatedBy)
            .HasMaxLength(256);

        builder.Property(tr => tr.UpdatedBy)
            .HasMaxLength(256);

        builder.HasOne(tr => tr.Language)
            .WithMany()
            .HasForeignKey(tr => tr.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tr => new { tr.LanguageId, tr.Module, tr.ResourceKey })
            .HasDatabaseName("IX_Translations_Language_Module_Key")
            .IsUnique();
    }
}
