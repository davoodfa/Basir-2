using Basir.Application.Auth.Interfaces;
using Basir.Domain.Enums;
using Basir.Domain.Interfaces.Repositories;

namespace Basir.Web.Middleware;

public class ThemeMiddleware
{
    private readonly RequestDelegate _next;

    public ThemeMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(
        HttpContext context,
        IThemeRepository themeRepo,
        IUserPreferenceRepository? preferenceRepo,
        ICurrentUserService currentUser)
    {
        var theme = await ResolveThemeAsync(context, themeRepo, preferenceRepo, currentUser);

        context.Items["Theme"] = theme?.Name ?? "light";
        context.Items["ThemeType"] = theme?.ThemeType.ToString() ?? "Light";
        context.Items["ThemeId"] = theme?.Id ?? 1;

        await _next(context);
    }

    private static async Task<Domain.Entities.Globalization.Theme?> ResolveThemeAsync(
        HttpContext context,
        IThemeRepository themeRepo,
        IUserPreferenceRepository? preferenceRepo,
        ICurrentUserService currentUser)
    {
        var cookie = context.Request.Cookies["Basir.Theme"];
        if (!string.IsNullOrEmpty(cookie))
        {
            var theme = await themeRepo.GetByNameAsync(cookie);
            if (theme is not null) return theme;
        }

        if (currentUser.IsAuthenticated && currentUser.UserId.HasValue && preferenceRepo is not null)
        {
            var pref = await preferenceRepo.GetByUserIdAsync(currentUser.UserId.Value);
            if (pref is not null)
            {
                var theme = await themeRepo.GetByIdAsync(pref.ThemeId);
                if (theme is not null) return theme;
            }
        }

        return await themeRepo.GetDefaultAsync();
    }
}

public static class ThemeMiddlewareExtensions
{
    public static IApplicationBuilder UseTheme(this IApplicationBuilder app)
        => app.UseMiddleware<ThemeMiddleware>();
}
