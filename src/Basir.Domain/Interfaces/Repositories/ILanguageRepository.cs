using Basir.Domain.Entities.Globalization;

namespace Basir.Domain.Interfaces.Repositories;

public interface ILanguageRepository
{
    Task<List<Language>> GetAllAsync(CancellationToken ct = default);
    Task<List<Language>> GetEnabledAsync(CancellationToken ct = default);
    Task<Language?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Language?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<Language?> GetDefaultAsync(CancellationToken ct = default);
}
