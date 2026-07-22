using Basir.Domain.Entities.Globalization;

namespace Basir.Domain.Interfaces.Repositories;

public interface IThemeRepository
{
    Task<List<Theme>> GetAllAsync(CancellationToken ct = default);
    Task<List<Theme>> GetEnabledAsync(CancellationToken ct = default);
    Task<Theme?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Theme?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<Theme?> GetDefaultAsync(CancellationToken ct = default);
}
