using Basir.Web.Resources;
using Basir.Web.Resources.Authentication;
using Basir.Web.Resources.Validation;
using Basir.Web.Resources.Errors;
using Basir.Web.Resources.Layout;
using Basir.Web.Resources.Navigation;
using Basir.Web.Resources.Project;
using Basir.Web.Resources.Editor;
using Basir.Web.Resources.AI;
using Microsoft.Extensions.Localization;

namespace Basir.Web.Extensions;

public static class LocalizationExtensions
{
    public static IServiceCollection AddBasirLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddTransient<IStringLocalizer<SharedResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<SharedResource>>());
        services.AddTransient<IStringLocalizer<AuthenticationResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<AuthenticationResource>>());
        services.AddTransient<IStringLocalizer<ValidationResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<ValidationResource>>());
        services.AddTransient<IStringLocalizer<ErrorsResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<ErrorsResource>>());
        services.AddTransient<IStringLocalizer<LayoutResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<LayoutResource>>());
        services.AddTransient<IStringLocalizer<NavigationResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<NavigationResource>>());
        services.AddTransient<IStringLocalizer<ProjectResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<ProjectResource>>());
        services.AddTransient<IStringLocalizer<EditorResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<EditorResource>>());
        services.AddTransient<IStringLocalizer<AIResource>>(sp =>
            sp.GetRequiredService<IStringLocalizer<AIResource>>());

        return services;
    }
}
