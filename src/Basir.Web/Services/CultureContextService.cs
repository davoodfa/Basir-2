using Basir.Application.Globalization;
using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Basir.Web.Services;

public class CultureContextService : ICultureContext
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IOptions<GlobalizationOptions> _options;

    public CultureContextService(IHttpContextAccessor accessor, IOptions<GlobalizationOptions> options)
    {
        _accessor = accessor;
        _options = options;
    }

    public string CurrentCulture
        => _accessor.HttpContext?.Items["Culture"]?.ToString() ?? _options.Value.DefaultCulture;

    public string CurrentLanguage
    {
        get
        {
            var lang = _accessor.HttpContext?.Items["Language"] as Domain.Entities.Globalization.Language;
            if (lang is not null)
                return lang.NativeName;

            var culture = CurrentCulture;
            return culture switch
            {
                "fa-IR" => "فارسی",
                "en-US" => "English",
                "ar-SA" => "العربية",
                _ => culture
            };
        }
    }

    public Direction Direction
        => _accessor.HttpContext?.Items["Direction"]?.ToString() == "rtl"
            ? Direction.Rtl
            : Direction.Ltr;

    public bool IsRtl => Direction == Direction.Rtl;

    public List<string> SupportedCultures => _options.Value.SupportedCultures;

    public List<string> SupportedUICultures => _options.Value.SupportedUICultures;
}
