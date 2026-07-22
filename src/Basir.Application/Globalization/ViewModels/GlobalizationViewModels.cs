using System.ComponentModel.DataAnnotations;
using Basir.Application.Globalization.DTOs;

namespace Basir.Application.Globalization.ViewModels;

public class PreferencesViewModel
{
    [Required]
    public int LanguageId { get; set; }

    [Required]
    public int ThemeId { get; set; }

    [Required]
    public string TimeZoneId { get; set; } = "UTC";

    [Required]
    public string CalendarType { get; set; } = "Gregorian";

    [Required]
    public string NumberFormat { get; set; } = "Western";

    public int FirstDayOfWeek { get; set; } = 6;

    [Range(5, 100)]
    public int ItemsPerPage { get; set; } = 20;

    public List<LanguageDto> Languages { get; set; } = [];
    public List<ThemeDto> Themes { get; set; } = [];
    public List<string> TimeZones { get; set; } = [];
}
