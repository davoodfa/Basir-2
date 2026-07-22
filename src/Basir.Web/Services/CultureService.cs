using System.Globalization;
using Basir.Application.Globalization;
using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Basir.Web.Services;

public class CultureService : ICultureService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GlobalizationOptions _options;

    public CultureService(IHttpContextAccessor httpContextAccessor, IOptions<GlobalizationOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    public string CurrentCulture
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.Items["Culture"] is string culture)
                return culture;
            return CultureInfo.CurrentCulture.Name ?? _options.DefaultRequestCulture.Culture;
        }
    }

    public string CurrentUICulture
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.Items["Culture"] is string culture)
                return culture;
            return CultureInfo.CurrentUICulture.Name ?? _options.DefaultRequestCulture.UICulture;
        }
    }

    public string CurrentLanguage
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.Items["Language"] is Domain.Entities.Globalization.Language lang)
                return lang.NativeName;
            return new CultureInfo(CurrentCulture).DisplayName;
        }
    }

    public Direction Direction
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.Items["Direction"]?.ToString() == "rtl")
                return Direction.Rtl;
            return Direction.Ltr;
        }
    }

    public bool IsRtl => Direction == Direction.Rtl;

    public List<string> SupportedCultures => _options.SupportedCultures;

    public List<string> SupportedUICultures => _options.SupportedUICultures;

    public async Task SetLanguageAsync(string cultureCode)
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx is null) return;

        if (!_options.SupportedCultures.Contains(cultureCode))
            cultureCode = _options.DefaultCulture;

        var cultureInfo = new CultureInfo(cultureCode);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        var direction = cultureInfo.TextInfo.IsRightToLeft ? "rtl" : "ltr";

        ctx.Items["Culture"] = cultureCode;
        ctx.Items["Direction"] = direction;

        ctx.Response.Cookies.Append("Basir.Culture", cultureCode, new CookieOptions
        {
            HttpOnly = false,
            Secure = ctx.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true
        });

        await Task.CompletedTask;
    }

    public string GetPersistedLanguage()
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx is null) return _options.DefaultCulture;

        var cookie = ctx.Request.Cookies["Basir.Culture"];
        if (!string.IsNullOrEmpty(cookie) && _options.SupportedCultures.Contains(cookie))
            return cookie;

        return _options.DefaultCulture;
    }
}
