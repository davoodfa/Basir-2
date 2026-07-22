using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Enums;

namespace Basir.Web.Services;

public class ThemeContextService : IThemeContext
{
    private readonly IHttpContextAccessor _accessor;

    public ThemeContextService(IHttpContextAccessor accessor) => _accessor = accessor;

    public string ThemeName
        => _accessor.HttpContext?.Items["Theme"]?.ToString() ?? "light";

    public ThemeType ThemeType
    {
        get
        {
            var type = _accessor.HttpContext?.Items["ThemeType"]?.ToString();
            return type switch
            {
                "Dark" => ThemeType.Dark,
                "Custom" => ThemeType.Custom,
                _ => ThemeType.Light
            };
        }
    }

    public bool IsDarkMode => ThemeType == ThemeType.Dark;
}
