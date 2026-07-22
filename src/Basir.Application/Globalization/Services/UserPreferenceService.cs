using Basir.Application.Globalization.DTOs;
using Basir.Application.Globalization.Interfaces;
using Basir.Application.Globalization.Mapping;
using Basir.Domain.Common;
using Basir.Domain.Interfaces.Repositories;

namespace Basir.Application.Globalization.Services;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IUserPreferenceRepository _repository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IThemeRepository _themeRepository;

    public UserPreferenceService(
        IUserPreferenceRepository repository,
        ILanguageRepository languageRepository,
        IThemeRepository themeRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
        _themeRepository = themeRepository;
    }

    public async Task<UserPreferenceDto?> GetAsync(Guid userId, CancellationToken ct = default)
    {
        var pref = await _repository.GetByUserIdAsync(userId, ct);
        return pref?.ToDto();
    }

    public async Task<Result<UserPreferenceDto>> UpdateAsync(Guid userId, UserPreferenceDto dto, CancellationToken ct = default)
    {
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId, ct);
        if (language is null)
            return Result<UserPreferenceDto>.Failure("Invalid language.");

        var theme = await _themeRepository.GetByIdAsync(dto.ThemeId, ct);
        if (theme is null)
            return Result<UserPreferenceDto>.Failure("Invalid theme.");

        var pref = await _repository.GetByUserIdAsync(userId, ct);
        if (pref is null)
        {
            pref = dto.ToEntity(userId);
            await _repository.AddAsync(pref, ct);
        }
        else
        {
            pref.ApplyDto(dto);
            pref.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(pref, ct);
        }

        return Result<UserPreferenceDto>.Success(pref.ToDto());
    }

    public async Task<Result> CreateDefaultAsync(Guid userId, CancellationToken ct = default)
    {
        var existing = await _repository.GetByUserIdAsync(userId, ct);
        if (existing is not null)
            return Result.Success();

        var defaultLang = await _languageRepository.GetDefaultAsync(ct);
        var defaultTheme = await _themeRepository.GetDefaultAsync(ct);

        var pref = new Domain.Entities.Globalization.UserPreference
        {
            UserId = userId,
            LanguageId = defaultLang?.Id ?? 1,
            ThemeId = defaultTheme?.Id ?? 1,
            TimeZoneId = "UTC",
            CalendarType = "Gregorian",
            NumberFormat = "Western",
            FirstDayOfWeek = 6,
            ItemsPerPage = 20
        };

        await _repository.AddAsync(pref, ct);
        return Result.Success();
    }
}
