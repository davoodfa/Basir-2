using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Interfaces.Repositories;

namespace Basir.Application.Globalization.Services;

public class LocalizationService : ILocalizationService
{
    private readonly ITranslationRepository _repository;
    private readonly ICultureContext _cultureContext;
    private readonly ILanguageRepository _languageRepository;

    public LocalizationService(
        ITranslationRepository repository,
        ICultureContext cultureContext,
        ILanguageRepository languageRepository)
    {
        _repository = repository;
        _cultureContext = cultureContext;
        _languageRepository = languageRepository;
    }

    public async Task<string> TranslateAsync(string module, string key, params object[] args)
    {
        var language = await ResolveLanguageAsync();
        if (language is null) return key;

        var value = await _repository.GetAsync(language.Id, module, key);
        if (value is null)
        {
            var defaultLang = await _languageRepository.GetDefaultAsync();
            if (defaultLang is not null && defaultLang.Id != language.Id)
                value = await _repository.GetAsync(defaultLang.Id, module, key);
        }

        var result = value ?? key;
        return args.Length > 0 ? string.Format(result, args) : result;
    }

    public string Translate(string module, string key, params object[] args)
    {
        var cultureCode = _cultureContext.CurrentCulture;
        return $"[{module}:{key}]";
    }

    private async Task<Domain.Entities.Globalization.Language?> ResolveLanguageAsync()
    {
        var code = _cultureContext.CurrentCulture;
        return await _languageRepository.GetByCodeAsync(code);
    }
}
