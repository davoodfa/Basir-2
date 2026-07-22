using System.Globalization;
using Basir.Application.Auth.Interfaces;
using Basir.Application.Globalization;
using Basir.Domain.Entities.Globalization;
using Basir.Domain.Enums;
using Basir.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace Basir.Web.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(
        HttpContext context,
        ILanguageRepository languageRepo,
        IUserPreferenceRepository? preferenceRepo,
        ICurrentUserService currentUser,
        IOptions<GlobalizationOptions> options)
    {
        var opt = options.Value;
        var language = await ResolveLanguageAsync(context, languageRepo, preferenceRepo, currentUser, opt);

        var cultureCode = language?.Code ?? opt.DefaultCulture;
        var cultureInfo = new CultureInfo(cultureCode);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        var direction = language?.Direction == Direction.Rtl ? "rtl" : "ltr";

        context.Items["Culture"] = cultureCode;
        context.Items["Direction"] = direction;
        context.Items["Language"] = language;

        await _next(context);
    }

    private static async Task<Language?> ResolveLanguageAsync(
        HttpContext context,
        ILanguageRepository languageRepo,
        IUserPreferenceRepository? preferenceRepo,
        ICurrentUserService currentUser,
        GlobalizationOptions options)
    {
        var cookie = context.Request.Cookies["Basir.Culture"];
        if (!string.IsNullOrEmpty(cookie))
        {
            var lang = await languageRepo.GetByCodeAsync(cookie);
            if (lang is not null) return lang;
        }

        if (currentUser.IsAuthenticated && currentUser.UserId.HasValue && preferenceRepo is not null)
        {
            var pref = await preferenceRepo.GetByUserIdAsync(currentUser.UserId.Value);
            if (pref is not null)
            {
                var lang = await languageRepo.GetByIdAsync(pref.LanguageId);
                if (lang is not null) return lang;
            }
        }

        if (options.EnableBrowserDetection)
        {
            var acceptLanguage = context.Request.Headers.AcceptLanguage.FirstOrDefault();
            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var preferred = acceptLanguage.Split(',')
                    .Select(x => x.Split(';')[0].Trim())
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(preferred))
                {
                    var lang = await languageRepo.GetByCodeAsync(preferred);
                    if (lang is not null) return lang;
                }
            }
        }

        return await languageRepo.GetDefaultAsync();
    }
}

public static class CultureMiddlewareExtensions
{
    public static IApplicationBuilder UseCulture(this IApplicationBuilder app)
        => app.UseMiddleware<CultureMiddleware>();
}
