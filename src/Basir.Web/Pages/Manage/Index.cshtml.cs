using Basir.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Manage;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ICurrentUserService _currentUser;

    public IndexModel(ICurrentUserService currentUser) => _currentUser = currentUser;

    public string? UserName => _currentUser.UserName;
    public string? Email => _currentUser.Email;
    public IReadOnlyList<string> Roles => _currentUser.Roles;

    public void OnGet()
    {
    }
}
