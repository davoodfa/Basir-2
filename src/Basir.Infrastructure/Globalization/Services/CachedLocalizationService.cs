using System.Threading;
using Basir.Application.Globalization;
using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Basir.Infrastructure.Globalization.Services;

public class CachedLocalizationService : ILocalizationService
{
    private readonly ITranslationRepository _repository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ICultureContext _cultureContext;
    private readonly IMemoryCache _cache;
    private readonly int _cacheDurationMinutes;

    public CachedLocalizationService(
        ITranslationRepository repository,
        ILanguageRepository languageRepository,
        ICultureContext cultureContext,
        IMemoryCache cache,
        IOptions<GlobalizationOptions> options)
    {
        _repository = repository;
        _languageRepository = languageRepository;
        _cultureContext = cultureContext;
        _cache = cache;
        _cacheDurationMinutes = options.Value.CacheDurationMinutes;
    }

    public string Translate(string module, string key, params object[] args)
    {
        var culture = _cultureContext.CurrentCulture;
        var cacheKey = $"l10n:{culture}:{module}:{key}";

        if (_cache.TryGetValue(cacheKey, out string? cached) && cached is not null)
            return args.Length > 0 ? string.Format(cached, args) : cached;

        var language = _languageRepository.GetByCodeAsync(culture).GetAwaiter().GetResult();
        if (language is null) return key;

        var value = _repository.GetAsync(language.Id, module, key).GetAwaiter().GetResult();
        if (value is null)
        {
            var defaultLang = _languageRepository.GetDefaultAsync().GetAwaiter().GetResult();
            if (defaultLang is not null && defaultLang.Id != language.Id)
                value = _repository.GetAsync(defaultLang.Id, module, key).GetAwaiter().GetResult();
        }

        value ??= key;
        _cache.Set(cacheKey, value, TimeSpan.FromMinutes(_cacheDurationMinutes));

        return args.Length > 0 ? string.Format(value, args) : value;
    }

    public async Task<string> TranslateAsync(string module, string key, params object[] args)
    {
        var culture = _cultureContext.CurrentCulture;
        var cacheKey = $"l10n:{culture}:{module}:{key}";

        if (_cache.TryGetValue(cacheKey, out string? cached) && cached is not null)
            return args.Length > 0 ? string.Format(cached, args) : cached;

        var language = await _languageRepository.GetByCodeAsync(culture);
        if (language is null) return key;

        var value = await _repository.GetAsync(language.Id, module, key);
        if (value is null)
        {
            var defaultLang = await _languageRepository.GetDefaultAsync();
            if (defaultLang is not null && defaultLang.Id != language.Id)
                value = await _repository.GetAsync(defaultLang.Id, module, key);
        }

        value ??= key;
        _cache.Set(cacheKey, value, TimeSpan.FromMinutes(_cacheDurationMinutes));

        return args.Length > 0 ? string.Format(value, args) : value;
    }
}
