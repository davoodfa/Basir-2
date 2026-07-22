using System.ComponentModel.DataAnnotations;
using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IAuthService _auth;
    private readonly SignInManager<Basir.Domain.Entities.Identity.ApplicationUser> _signIn;
    private readonly IValidator<LoginDto> _validator;

    public LoginModel(
        IAuthService auth,
        SignInManager<Basir.Domain.Entities.Identity.ApplicationUser> signIn,
        IValidator<LoginDto> validator)
    {
        _auth = auth;
        _signIn = signIn;
        _validator = validator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public void OnGet(string? returnUrl = null) => ReturnUrl = returnUrl;

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
            return Page();

        var dto = new LoginDto(Input.Email, Input.Password, Input.RememberMe);
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return Page();
        }

        var result = await _auth.LoginAsync(dto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            return Page();
        }

        var user = await _signIn.UserManager.FindByEmailAsync(Input.Email);
        if (user is not null)
            await _signIn.SignInAsync(user, Input.RememberMe);

        return LocalRedirect(returnUrl);
    }
}
