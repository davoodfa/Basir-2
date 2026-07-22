using Basir.Application.Globalization.DTOs;
using Basir.Application.Globalization.Interfaces;
using Basir.Application.Globalization.Mapping;
using Basir.Domain.Common;
using Basir.Domain.Interfaces.Repositories;

namespace Basir.Application.Globalization.Services;

public class ThemeService : IThemeService
{
    private readonly IThemeRepository _repository;
    private readonly IUserPreferenceRepository _preferenceRepository;

    public ThemeService(IThemeRepository repository, IUserPreferenceRepository preferenceRepository)
    {
        _repository = repository;
        _preferenceRepository = preferenceRepository;
    }

    public async Task<List<ThemeDto>> GetAllAsync(CancellationToken ct = default)
    {
        var themes = await _repository.GetEnabledAsync(ct);
        return themes.Select(t => t.ToDto()).ToList();
    }

    public async Task<ThemeDto?> GetDefaultAsync(CancellationToken ct = default)
    {
        var theme = await _repository.GetDefaultAsync(ct);
        return theme?.ToDto();
    }

    public async Task<Result> SetPreferenceAsync(Guid userId, int themeId, CancellationToken ct = default)
    {
        var theme = await _repository.GetByIdAsync(themeId, ct);
        if (theme is null)
            return Result.Failure("Theme not found.");

        var pref = await _preferenceRepository.GetByUserIdAsync(userId, ct);
        if (pref is null)
            return Result.Failure("User preferences not found.");

        pref.ThemeId = themeId;
        pref.UpdatedAt = DateTime.UtcNow;
        await _preferenceRepository.UpdateAsync(pref, ct);

        return Result.Success();
    }
}
