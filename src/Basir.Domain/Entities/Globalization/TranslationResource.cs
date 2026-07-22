using Basir.Domain.Common;

namespace Basir.Domain.Entities.Globalization;

public class TranslationResource : IAuditable
{
    public long Id { get; set; }
    public int LanguageId { get; set; }
    public string Module { get; set; } = string.Empty;
    public string ResourceKey { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public Language Language { get; set; } = null!;
}
