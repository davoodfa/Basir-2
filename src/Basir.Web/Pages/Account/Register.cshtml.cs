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
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 12)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone]
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
