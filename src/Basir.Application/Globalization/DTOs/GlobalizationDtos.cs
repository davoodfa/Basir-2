namespace Basir.Application.Globalization.DTOs;

public class LanguageDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
    public string Direction { get; set; } = "ltr";
    public string? FlagIcon { get; set; }
    public bool IsDefault { get; set; }
}

public class ThemeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string ThemeType { get; set; } = "Light";
    public string PrimaryColor { get; set; } = string.Empty;
    public string? SecondaryColor { get; set; }
    public bool IsDefault { get; set; }
    public string? CssVariables { get; set; }
}

public class UserPreferenceDto
{
    public int LanguageId { get; set; }
    public int ThemeId { get; set; }
    public string TimeZoneId { get; set; } = "UTC";
    public string CalendarType { get; set; } = "Gregorian";
    public string NumberFormat { get; set; } = "Western";
    public int FirstDayOfWeek { get; set; } = 6;
    public int ItemsPerPage { get; set; } = 20;
}

public class TranslationDto
{
    public int LanguageId { get; set; }
    public string Module { get; set; } = string.Empty;
    public string ResourceKey { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
