using System.Text.Json;
using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Basir.Web.TagHelpers;

[HtmlTargetElement("theme-styles")]
public class ThemeStylesTagHelper : TagHelper
{
    private readonly IThemeContext _themeContext;
    private readonly IThemeRepository _themeRepository;

    public ThemeStylesTagHelper(IThemeContext themeContext, IThemeRepository themeRepository)
    {
        _themeContext = themeContext;
        _themeRepository = themeRepository;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "style";

        var theme = await _themeRepository.GetByNameAsync(_themeContext.ThemeName);
        if (theme is null)
        {
            output.Content.SetHtmlContent(":root {}");
            return;
        }

        var css = $@":root {{
    --theme-primary: {theme.PrimaryColor};
    --theme-secondary: {theme.SecondaryColor ?? theme.PrimaryColor};
}}";

        if (!string.IsNullOrEmpty(theme.CssVariables))
        {
            try
            {
                var vars = JsonSerializer.Deserialize<Dictionary<string, string>>(theme.CssVariables);
                if (vars is not null)
                {
                    foreach (var (key, value) in vars)
                    {
                        css += $"\n    --{key}: {value};";
                    }
                }
            }
            catch
            {
            }
        }

        output.Content.SetHtmlContent(css);
    }
}
