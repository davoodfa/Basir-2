using Basir.Application.Auth.Interfaces;
using Basir.Application.Globalization.DTOs;
using Basir.Application.Globalization.Interfaces;
using Basir.Application.Globalization.Mapping;
using Basir.Application.Globalization.ViewModels;
using Basir.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basir.Web.Pages.Manage;

[Authorize]
public class PreferencesModel : PageModel
{
    private readonly IUserPreferenceService _preferenceService;
    private readonly ILanguageRepository _languageRepository;
    private readonly IThemeRepository _themeRepository;
    private readonly ICurrentUserService _currentUser;

    public PreferencesModel(
        IUserPreferenceService preferenceService,
        ILanguageRepository languageRepository,
        IThemeRepository themeRepository,
        ICurrentUserService currentUser)
    {
        _preferenceService = preferenceService;
        _languageRepository = languageRepository;
        _themeRepository = themeRepository;
        _currentUser = currentUser;
    }

    [BindProperty]
    public PreferencesViewModel Preferences { get; set; } = new();

    public bool Saved { get; set; }

    public List<LanguageDto> Languages { get; set; } = [];
    public List<ThemeDto> Themes { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.UserId.HasValue) return Challenge();

        await LoadDropdownsAsync();

        var dto = await _preferenceService.GetAsync(_currentUser.UserId.Value);
        if (dto is not null)
        {
            Preferences = dto.ToViewModel();
        }

        Preferences.Languages = Languages;
        Preferences.Themes = Themes;
        Preferences.TimeZones = GetTimeZones();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_currentUser.UserId.HasValue) return Challenge();

        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            Preferences.Languages = Languages;
            Preferences.Themes = Themes;
            Preferences.TimeZones = GetTimeZones();
            return Page();
        }

        var dto = Preferences.ToDto();
        var result = await _preferenceService.UpdateAsync(_currentUser.UserId.Value, dto);

        if (!result.Succeeded)
        {
            await LoadDropdownsAsync();
            Preferences.Languages = Languages;
            Preferences.Themes = Themes;
            Preferences.TimeZones = GetTimeZones();

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);

            return Page();
        }

        Saved = true;
        await LoadDropdownsAsync();
        Preferences.Languages = Languages;
        Preferences.Themes = Themes;
        Preferences.TimeZones = GetTimeZones();

        return Page();
    }

    private async Task LoadDropdownsAsync()
    {
        var languages = await _languageRepository.GetEnabledAsync();
        Languages = languages.Select(l => l.ToDto()).ToList();

        var themes = await _themeRepository.GetEnabledAsync();
        Themes = themes.Select(t => t.ToDto()).ToList();
    }

    private static List<string> GetTimeZones()
    {
        return TimeZoneInfo.GetSystemTimeZones()
            .Select(tz => tz.Id)
            .OrderBy(id => id)
            .ToList();
    }
}
