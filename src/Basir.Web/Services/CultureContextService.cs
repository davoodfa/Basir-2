using Basir.Application.Globalization.Interfaces;
using Basir.Domain.Enums;

namespace Basir.Web.Services;

public class CultureContextService : ICultureContext
{
    private readonly IHttpContextAccessor _accessor;

    public CultureContextService(IHttpContextAccessor accessor) => _accessor = accessor;

    public string CurrentCulture
        => _accessor.HttpContext?.Items["Culture"]?.ToString() ?? "fa-IR";

    public string CurrentLanguage
    {
        get
        {
            var culture = CurrentCulture;
            return culture switch
            {
                "fa-IR" => "فارسی",
                "en-US" => "English",
                _ => culture
            };
        }
    }

    public Direction Direction
        => _accessor.HttpContext?.Items["Direction"]?.ToString() == "rtl"
            ? Direction.Rtl
            : Direction.Ltr;

    public bool IsRtl => Direction == Direction.Rtl;
}
