using Basir.Domain.Enums;

namespace Basir.Application.Globalization.Interfaces;

public interface ICultureContext
{
    string CurrentCulture { get; }
    string CurrentLanguage { get; }
    Direction Direction { get; }
    bool IsRtl { get; }
}
