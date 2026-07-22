namespace Basir.Domain.Entities.Globalization;

public class UserPreference
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
    public int ThemeId { get; set; }
    public string TimeZoneId { get; set; } = "UTC";
    public string CalendarType { get; set; } = "Gregorian";
    public string NumberFormat { get; set; } = "Western";
    public int FirstDayOfWeek { get; set; } = 6;
    public int ItemsPerPage { get; set; } = 20;
    public DateTime? UpdatedAt { get; set; }

    public Language Language { get; set; } = null!;
    public Theme Theme { get; set; } = null!;
}
