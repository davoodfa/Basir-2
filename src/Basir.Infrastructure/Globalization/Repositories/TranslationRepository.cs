using Basir.Domain.Entities.Globalization;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Globalization.Repositories;

public class TranslationRepository : ITranslationRepository
{
    private readonly AppIdentityDbContext _context;

    public TranslationRepository(AppIdentityDbContext context) => _context = context;

    public async Task<string?> GetAsync(int languageId, string module, string key, CancellationToken ct = default)
    {
        var translation = await _context.Set<TranslationResource>()
            .Where(tr => tr.LanguageId == languageId && tr.Module == module && tr.ResourceKey == key)
            .Select(tr => tr.Value)
            .FirstOrDefaultAsync(ct);

        return translation;
    }

    public async Task<Dictionary<string, string>> GetModuleAsync(int languageId, string module, CancellationToken ct = default)
    {
        return await _context.Set<TranslationResource>()
            .Where(tr => tr.LanguageId == languageId && tr.Module == module)
            .ToDictionaryAsync(tr => tr.ResourceKey, tr => tr.Value, ct);
    }

    public async Task<List<TranslationResource>> GetAllByLanguageAsync(int languageId, CancellationToken ct = default)
    {
        return await _context.Set<TranslationResource>()
            .Where(tr => tr.LanguageId == languageId)
            .ToListAsync(ct);
    }
}
