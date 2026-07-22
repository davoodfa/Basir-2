using System.ComponentModel.DataAnnotations;
using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using Basir.Application.Auth.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basir.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IAuthService _auth;
    private readonly IValidator<RegisterDto> _validator;

    public RegisterModel(IAuthService auth, IValidator<RegisterDto> validator)
    {
        _auth = auth;
        _validator = validator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین ۳ تا ۵۰ کاراکتر باشد.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام الزامی است.")]
        [StringLength(50, ErrorMessage = "نام حداکثر ۵۰ کاراکتر می‌تواند باشد.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
        [StringLength(50, ErrorMessage = "نام خانوادگی حداکثر ۵۰ کاراکتر می‌تواند باشد.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [StringLength(100, MinimumLength = 12, ErrorMessage = "رمز عبور باید حداقل ۱۲ کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone(ErrorMessage = "شماره تلفن معتبر وارد کنید.")]
        public string? PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var dto = new RegisterDto(
            Input.Email, Input.UserName, Input.FirstName, Input.LastName,
            Input.Password, Input.ConfirmPassword, Input.PhoneNumber);

        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return Page();
        }

        var result = await _auth.RegisterAsync(dto);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            return Page();
        }

        return RedirectToPage("/Account/RegisterConfirmation", new { email = Input.Email });
    }
}
