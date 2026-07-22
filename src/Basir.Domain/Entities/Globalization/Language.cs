using Basir.Domain.Common;
using Basir.Domain.Enums;

namespace Basir.Domain.Entities.Globalization;

public class Language : IAuditable
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
    public Direction Direction { get; set; }
    public string? FlagIcon { get; set; }
    public bool IsDefault { get; set; }
    public bool IsEnabled { get; set; } = true;
    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
