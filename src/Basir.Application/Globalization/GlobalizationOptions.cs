namespace Basir.Application.Globalization;

public class GlobalizationOptions
{
    public const string SectionName = "Globalization";

    public string DefaultCulture { get; set; } = "fa-IR";
    public int CacheDurationMinutes { get; set; } = 60;
    public bool EnableBrowserDetection { get; set; } = true;
}
