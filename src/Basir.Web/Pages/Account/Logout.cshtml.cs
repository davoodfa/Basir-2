using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<Basir.Domain.Entities.Identity.ApplicationUser> _signIn;

    public LogoutModel(SignInManager<Basir.Domain.Entities.Identity.ApplicationUser> signIn)
        => _signIn = signIn;

    public async Task<IActionResult> OnPostAsync()
    {
        await _signIn.SignOutAsync();
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await _signIn.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
