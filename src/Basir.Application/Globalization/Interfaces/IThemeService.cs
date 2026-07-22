using Basir.Application.Globalization.DTOs;
using Basir.Domain.Common;

namespace Basir.Application.Globalization.Interfaces;

public interface IThemeService
{
    Task<List<ThemeDto>> GetAllAsync(CancellationToken ct = default);
    Task<ThemeDto?> GetDefaultAsync(CancellationToken ct = default);
    Task<Result> SetPreferenceAsync(Guid userId, int themeId, CancellationToken ct = default);
}
