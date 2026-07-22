using Basir.Domain.Entities.Globalization;
using Basir.Domain.Entities.Identity;
using Basir.Infrastructure.Globalization.Persistence;
using Basir.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Identity;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Theme> Themes => Set<Theme>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();
    public DbSet<TranslationResource> Translations => Set<TranslationResource>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new ApplicationRoleConfiguration());
        builder.ApplyConfiguration(new LanguageConfiguration());
        builder.ApplyConfiguration(new ThemeConfiguration());
        builder.ApplyConfiguration(new UserPreferenceConfiguration());
        builder.ApplyConfiguration(new TranslationResourceConfiguration());
    }
}
