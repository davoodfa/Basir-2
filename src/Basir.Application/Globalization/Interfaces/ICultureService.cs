using Basir.Domain.Enums;

namespace Basir.Application.Globalization.Interfaces;

public interface ICultureService
{
    string CurrentCulture { get; }
    string CurrentUICulture { get; }
    string CurrentLanguage { get; }
    Direction Direction { get; }
    bool IsRtl { get; }
    List<string> SupportedCultures { get; }
    List<string> SupportedUICultures { get; }
    Task SetLanguageAsync(string cultureCode);
    string GetPersistedLanguage();
}
