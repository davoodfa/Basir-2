using System.Security.Claims;
using Basir.Application.Auth.Interfaces;

namespace Basir.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User?.FindFirstValue("sub");
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? UserName
    {
        get
        {
            return User?.FindFirstValue(ClaimTypes.Name)
                   ?? User?.FindFirstValue("unique_name");
        }
    }

    public string? Email
    {
        get
        {
            return User?.FindFirstValue(ClaimTypes.Email)
                   ?? User?.FindFirstValue("email");
        }
    }

    public IReadOnlyList<string> Roles
    {
        get
        {
            var roles = new List<string>();
            if (User is null) return roles;

            roles.AddRange(User.FindAll(ClaimTypes.Role).Select(c => c.Value));

            if (User.FindFirst("role") is not null)
                roles.AddRange(User.FindAll("role").Select(c => c.Value));

            return roles.Distinct().ToList();
        }
    }
}
