using Basir.Domain.Enums;

namespace Basir.Application.Globalization.Interfaces;

public interface IThemeContext
{
    string ThemeName { get; }
    ThemeType ThemeType { get; }
    bool IsDarkMode { get; }
}
