namespace Basir.Application.Globalization.Interfaces;

public interface ILocalizationService
{
    Task<string> TranslateAsync(string module, string key, params object[] args);
    string Translate(string module, string key, params object[] args);
}
