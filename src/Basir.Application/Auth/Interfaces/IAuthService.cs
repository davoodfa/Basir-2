using Basir.Application.Auth.DTOs;
using Basir.Domain.Common;

namespace Basir.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto, CancellationToken ct = default);
    Task<Result<AuthResultDto>> LoginAsync(LoginDto dto, CancellationToken ct = default);
    Task<Result> LogoutAsync(Guid? userId, CancellationToken ct = default);
    Task<Result<TokenDto>> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken ct = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken ct = default);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailDto dto, CancellationToken ct = default);
    Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct = default);
    Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default);
    Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken ct = default);
    Task<Result> EnableTwoFactorAsync(Guid userId, CancellationToken ct = default);
}
