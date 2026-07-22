using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.ViewModels;
using Basir.Domain.Entities.Identity;

namespace Basir.Application.Auth.Mapping;

public static class AuthMapping
{
    public static AuthResultDto ToResult(this ApplicationUser user, IReadOnlyList<string> roles, TokenDto? tokens, DateTime? refreshTokenExpiresAt = null)
        => new(
            user.Id,
            user.UserName ?? user.Email!,
            user.Email!,
            roles,
            tokens,
            refreshTokenExpiresAt);

    public static CurrentUserDto ToCurrentUser(this ApplicationUser user, IReadOnlyList<string> roles)
        => new(user.Id, user.UserName ?? user.Email!, user.Email!, roles);

    public static LoginDto ToDto(this LoginViewModel viewModel)
        => new(viewModel.Email, viewModel.Password, viewModel.RememberMe);

    public static RegisterDto ToDto(this RegisterViewModel viewModel)
        => new(
            viewModel.Email,
            viewModel.UserName,
            viewModel.FirstName,
            viewModel.LastName,
            viewModel.Password,
            viewModel.ConfirmPassword,
            viewModel.PhoneNumber);

    public static ForgotPasswordDto ToDto(this ForgotPasswordViewModel viewModel)
        => new(viewModel.Email);

    public static ResetPasswordDto ToDto(this ResetPasswordViewModel viewModel)
        => new(viewModel.Email, viewModel.Token, viewModel.NewPassword, viewModel.ConfirmPassword);

    public static ChangePasswordDto ToDto(this ChangePasswordViewModel viewModel)
        => new(viewModel.CurrentPassword, viewModel.NewPassword, viewModel.ConfirmPassword);

    public static ConfirmEmailDto ToDto(this ConfirmEmailViewModel viewModel)
        => new(Guid.Parse(viewModel.UserId), viewModel.Token);

    public static ResendConfirmationEmailDto ToDto(this ResendConfirmationViewModel viewModel)
        => new(viewModel.Email);
}
