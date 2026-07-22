using System.ComponentModel.DataAnnotations;
using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class ResetPasswordModel : PageModel
{
    private readonly IAuthService _auth;
    private readonly IValidator<ResetPasswordDto> _validator;

    public ResetPasswordModel(IAuthService auth, IValidator<ResetPasswordDto> validator)
    {
        _auth = auth;
        _validator = validator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool Done { get; set; }

    public class InputModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور جدید الزامی است.")]
        [StringLength(100, MinimumLength = 12, ErrorMessage = "رمز عبور باید حداقل ۱۲ کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public void OnGet(string email, string token)
    {
        Input.Email = email;
        Input.Token = token;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var dto = new ResetPasswordDto(Input.Email, Input.Token, Input.NewPassword, Input.ConfirmPassword);
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return Page();
        }

        var result = await _auth.ResetPasswordAsync(dto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            return Page();
        }

        Done = true;
        return Page();
    }
}
