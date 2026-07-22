using Basir.Domain.Enums;

namespace Basir.Application.Globalization;

public class GlobalizationOptions
{
    public const string SectionName = "Globalization";

    public string DefaultCulture { get; set; } = "fa-IR";
    public int CacheDurationMinutes { get; set; } = 60;
    public bool EnableBrowserDetection { get; set; } = true;

    public List<string> SupportedCultures { get; set; } = new() { "fa-IR", "en-US", "ar-SA" };
    public List<string> SupportedUICultures { get; set; } = new() { "fa-IR", "en-US", "ar-SA" };
    public DefaultRequestCultureConfig DefaultRequestCulture { get; set; } = new();
}

public class DefaultRequestCultureConfig
{
    public string Culture { get; set; } = "fa-IR";
    public string UICulture { get; set; } = "fa-IR";
}
