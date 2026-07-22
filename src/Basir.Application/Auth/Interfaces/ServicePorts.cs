using Basir.Application.Auth.DTOs;
using Basir.Domain.Common;

namespace Basir.Application.Auth.Interfaces;

public interface ITokenService
{
    TokenDto GenerateToken(string userId, string userName, IReadOnlyList<string> roles);
    Task<Result<TokenDto>> RefreshAsync(Guid userId, string refreshToken, CancellationToken ct = default);
    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId, CancellationToken ct = default);
    Task<string> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default);
    Task<string> GenerateTwoFactorTokenAsync(Guid userId, CancellationToken ct = default);
}

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
}

public interface IEmailService
{
    Task SendConfirmationAsync(string email, string link, CancellationToken ct = default);
    Task SendPasswordResetAsync(string email, string link, CancellationToken ct = default);
    Task SendTwoFactorCodeAsync(string email, string code, CancellationToken ct = default);
}

public interface ISmsService
{
    Task SendTwoFactorCodeAsync(string phoneNumber, string code, CancellationToken ct = default);
}
