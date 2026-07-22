using System.ComponentModel.DataAnnotations;
using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly IAuthService _auth;
    private readonly IValidator<ForgotPasswordDto> _validator;

    public ForgotPasswordModel(IAuthService auth, IValidator<ForgotPasswordDto> validator)
    {
        _auth = auth;
        _validator = validator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool Sent { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید.")]
        public string Email { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var dto = new ForgotPasswordDto(Input.Email);
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return Page();
        }

        await _auth.ForgotPasswordAsync(dto);
        Sent = true;
        return Page();
    }
}
