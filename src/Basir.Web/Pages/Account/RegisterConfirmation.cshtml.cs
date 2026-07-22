using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class RegisterConfirmationModel : PageModel
{
    private readonly IAuthService _auth;

    public RegisterConfirmationModel(IAuthService auth) => _auth = auth;

    public string? Email { get; set; }

    public void OnGet(string? email)
    {
        Email = email;
    }

    public async Task<IActionResult> OnPostResendAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToPage("/Account/Register");

        await _auth.ResendConfirmationEmailAsync(new ResendConfirmationEmailDto(email));
        return RedirectToPage(new { email });
    }
}
