using Basir.Domain.Common;
using Basir.Domain.Enums;

namespace Basir.Domain.Entities.Globalization;

public class Theme : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public ThemeType ThemeType { get; set; }
    public string PrimaryColor { get; set; } = string.Empty;
    public string? SecondaryColor { get; set; }
    public bool IsDefault { get; set; }
    public bool IsEnabled { get; set; } = true;
    public string? CssVariables { get; set; }
    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
