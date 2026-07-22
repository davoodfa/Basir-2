using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly IAuthService _auth;

    public ConfirmEmailModel(IAuthService auth) => _auth = auth;

    public bool Succeeded { get; set; }
    public bool Invalid { get; set; }

    public async Task<IActionResult> OnGetAsync(string? userId, string? token)
    {
        if (!Guid.TryParse(userId, out var id) || string.IsNullOrEmpty(token))
        {
            Invalid = true;
            return Page();
        }

        var result = await _auth.ConfirmEmailAsync(new ConfirmEmailDto(id, token));
        Succeeded = result.Succeeded;
        Invalid = !result.Succeeded;
        return Page();
    }
}
