using Basir.Application.Auth.DTOs;
using Basir.Domain.Services;
using FluentValidation;

namespace Basir.Application.Auth.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .Matches("^[a-zA-Z0-9_.-]+$")
            .WithMessage("Username may only contain letters, numbers, dots, underscores, and hyphens.");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(UserDomainRules.Password.MinimumLength)
            .Matches(UserDomainRules.Password.UppercasePattern).WithMessage("Password must contain at least one uppercase letter.")
            .Matches(UserDomainRules.Password.LowercasePattern).WithMessage("Password must contain at least one lowercase letter.")
            .Matches(UserDomainRules.Password.DigitPattern).WithMessage("Password must contain at least one digit.")
            .Matches(UserDomainRules.Password.SpecialCharPattern).WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be a valid E.164 format (e.g., +1234567890).");
    }
}

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
        RuleFor(x => x.Token)
            .NotEmpty();
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(UserDomainRules.Password.MinimumLength)
            .Matches(UserDomainRules.Password.UppercasePattern).WithMessage("Password must contain at least one uppercase letter.")
            .Matches(UserDomainRules.Password.LowercasePattern).WithMessage("Password must contain at least one lowercase letter.")
            .Matches(UserDomainRules.Password.DigitPattern).WithMessage("Password must contain at least one digit.")
            .Matches(UserDomainRules.Password.SpecialCharPattern).WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("Passwords do not match.");
    }
}

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(UserDomainRules.Password.MinimumLength)
            .Matches(UserDomainRules.Password.UppercasePattern).WithMessage("Password must contain at least one uppercase letter.")
            .Matches(UserDomainRules.Password.LowercasePattern).WithMessage("Password must contain at least one lowercase letter.")
            .Matches(UserDomainRules.Password.DigitPattern).WithMessage("Password must contain at least one digit.")
            .Matches(UserDomainRules.Password.SpecialCharPattern).WithMessage("Password must contain at least one special character.")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New password must be different from current password.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("Passwords do not match.");
    }
}

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDto>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}

public class ResendConfirmationValidator : AbstractValidator<ResendConfirmationEmailDto>
{
    public ResendConfirmationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}
