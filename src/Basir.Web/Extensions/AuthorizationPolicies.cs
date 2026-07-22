using Microsoft.AspNetCore.Authorization;

namespace Basir.Web.Extensions;

public static class AuthorizationPolicies
{
    public const string Admin = "Admin";
    public const string Moderator = "Moderator";
    public const string User = "User";
    public const string AdminOrModerator = "AdminOrModerator";
    public const string Authenticated = "Authenticated";

    public static AuthorizationOptions AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Admin, policy =>
            policy.RequireRole(Admin)
                  .RequireAuthenticatedUser());

        options.AddPolicy(Moderator, policy =>
            policy.RequireRole(Moderator)
                  .RequireAuthenticatedUser());

        options.AddPolicy(User, policy =>
            policy.RequireRole(User)
                  .RequireAuthenticatedUser());

        options.AddPolicy(AdminOrModerator, policy =>
            policy.RequireRole(Admin, Moderator)
                  .RequireAuthenticatedUser());

        options.AddPolicy(Authenticated, policy =>
            policy.RequireAuthenticatedUser());

        return options;
    }
}
