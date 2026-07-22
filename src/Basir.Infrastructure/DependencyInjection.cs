using Basir.Application.Auth;
using Basir.Application.Auth.Interfaces;
using Basir.Application.Auth.Services;
using Basir.Application.Globalization;
using Basir.Application.Globalization.Interfaces;
using Basir.Application.Globalization.Services;
using Basir.Domain.Entities.Identity;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Auth;
using Basir.Infrastructure.Globalization.Repositories;
using Basir.Infrastructure.Globalization.Seed;
using Basir.Infrastructure.Globalization.Services;
using Basir.Infrastructure.Identity;
using Basir.Infrastructure.Options;
using Basir.Infrastructure.Persistence.Interceptors;
using Basir.Infrastructure.Repositories;
using Basir.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Basir.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        AddPersistence(services, config);
        AddIdentity(services);
        AddOptions(services, config);
        AddJwt(services);
        AddRepositories(services);
        AddServices(services);
        AddGlobalization(services, config);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<AppIdentityDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
            options.AddInterceptors(interceptor);
        });
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Stores.ProtectPersonalData = false;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddOptions(IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
        services.Configure<AuthOptions>(config.GetSection(AuthOptions.SectionName));
        services.Configure<GlobalizationOptions>(config.GetSection(GlobalizationOptions.SectionName));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AuthOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<GlobalizationOptions>>().Value);
    }

    private static void AddJwt(IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var jwt = sp.GetRequiredService<JwtOptions>();
            return JwtTokenValidator.CreateValidationParameters(jwt);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailSender>();
        services.AddScoped<ISmsService, SmsSender>();
        services.AddValidatorsFromAssemblyContaining<Application.Auth.Validators.RegisterValidator>();
    }

    private static void AddGlobalization(IServiceCollection services, IConfiguration config)
    {
        services.AddMemoryCache();

        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IThemeRepository, ThemeRepository>();
        services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
        services.AddScoped<ITranslationRepository, TranslationRepository>();

        services.AddScoped<ILocalizationService, CachedLocalizationService>();
        services.AddScoped<IThemeService, ThemeService>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        services.AddScoped<GlobalizationSeeder>();
    }
}
