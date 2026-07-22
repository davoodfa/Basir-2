using Basir.Application.Globalization.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Basir.Web.TagHelpers;

[HtmlTargetElement("localize")]
public class LocalizeTagHelper : TagHelper
{
    private readonly ILocalizationService _localization;

    public LocalizeTagHelper(ILocalizationService localization) => _localization = localization;

    [HtmlAttributeName("lc-module")]
    public string Module { get; set; } = string.Empty;

    [HtmlAttributeName("lc-key")]
    public string Key { get; set; } = string.Empty;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;

        if (string.IsNullOrWhiteSpace(Module) || string.IsNullOrWhiteSpace(Key))
        {
            output.Content.SetContent("[localize:missing-attributes]");
            return;
        }

        var text = await _localization.TranslateAsync(Module, Key);
        output.Content.SetContent(text);
    }
}
