using Basir.Domain.Entities.Globalization;

namespace Basir.Domain.Interfaces.Repositories;

public interface ITranslationRepository
{
    Task<string?> GetAsync(int languageId, string module, string key, CancellationToken ct = default);
    Task<Dictionary<string, string>> GetModuleAsync(int languageId, string module, CancellationToken ct = default);
    Task<List<TranslationResource>> GetAllByLanguageAsync(int languageId, CancellationToken ct = default);
}
