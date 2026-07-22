using System.ComponentModel.DataAnnotations;
using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Manage;

[Authorize]
public class ChangePasswordModel : PageModel
{
    private readonly IAuthService _auth;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<ChangePasswordDto> _validator;

    public ChangePasswordModel(
        IAuthService auth,
        ICurrentUserService currentUser,
        IValidator<ChangePasswordDto> validator)
    {
        _auth = auth;
        _currentUser = currentUser;
        _validator = validator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool Done { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "رمز عبور فعلی الزامی است.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور جدید الزامی است.")]
        [StringLength(100, MinimumLength = 12, ErrorMessage = "رمز عبور باید حداقل ۱۲ کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (_currentUser.UserId is null)
            return Challenge();

        if (!ModelState.IsValid)
            return Page();

        var dto = new ChangePasswordDto(Input.CurrentPassword, Input.NewPassword, Input.ConfirmPassword);
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return Page();
        }

        var result = await _auth.ChangePasswordAsync(
            _currentUser.UserId.Value, dto);

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
