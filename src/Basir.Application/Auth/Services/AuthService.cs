using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using Basir.Application.Auth.Mapping;
using Basir.Domain.Common;
using Basir.Domain.Entities.Identity;
using Basir.Domain.Enums;
using Basir.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Basir.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;
    private readonly IEmailService _email;
    private readonly IUnitOfWork _uow;
    private readonly AuthOptions _options;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository users,
        ITokenService tokens,
        IEmailService email,
        IUnitOfWork uow,
        AuthOptions options,
        ILogger<AuthService> logger)
    {
        _users = users;
        _tokens = tokens;
        _email = email;
        _uow = uow;
        _options = options;
        _logger = logger;
    }

    public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
    {
        var normalizedEmail = dto.Email.ToUpperInvariant();
        if (await _users.IsEmailTakenAsync(normalizedEmail, null, ct))
            return Result<AuthResultDto>.Failure("Email is already registered.");

        var normalizedUserName = dto.UserName.ToUpperInvariant();
        var existingUser = await _users.GetByUserNameAsync(normalizedUserName, ct);
        if (existingUser is not null)
            return Result<AuthResultDto>.Failure("Username is already taken.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            NormalizedUserName = normalizedUserName,
            Email = dto.Email,
            NormalizedEmail = normalizedEmail,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Status = UserStatus.Pending,
            EmailConfirmed = false
        };

        if (!await _users.CreateWithPasswordAsync(user, dto.Password, ct))
            return Result<AuthResultDto>.Failure(
                "Unable to create user. The password may not meet security requirements.");

        var token = await _tokens.GenerateEmailConfirmationTokenAsync(user.Id, ct);
        var link = $"{_options.ConfirmEmailBaseUrl}?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        await _email.SendConfirmationAsync(user.Email!, link, ct);

        var roles = await _users.GetRolesAsync(user.Id, ct);
        _logger.LogInformation("User {Email} registered successfully with id {UserId}", user.Email, user.Id);

        return Result<AuthResultDto>.Success(user.ToResult(roles, null));
    }

    public async Task<Result<AuthResultDto>> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(dto.Email.ToUpperInvariant(), ct);
        if (user is null)
            return Result<AuthResultDto>.Failure("Invalid credentials.");

        if (!user.CanLogin())
        {
            if (user.Status is UserStatus.Locked or UserStatus.Suspended)
                return Result<AuthResultDto>.Failure(
                    "Account is locked. Please try again later or reset your password.");

            if (!user.EmailConfirmed)
                return Result<AuthResultDto>.Failure(
                    "Email is not confirmed. Please check your inbox for the confirmation link.");

            if (user.LockoutEnd > DateTimeOffset.UtcNow)
                return Result<AuthResultDto>.Failure(
                    "Account is temporarily locked due to too many failed attempts. Please try again later.");

            return Result<AuthResultDto>.Failure("Account is not available for login.");
        }

        if (!await _users.CheckPasswordAsync(user.Id, dto.Password, ct))
        {
            user.RecordFailedLoginAttempt(_options.MaxFailedAccessAttempts);
            await _uow.SaveChangesAsync(ct);

            _logger.LogWarning(
                "Failed login attempt for user {Email}. Failed attempts: {Count}",
                dto.Email, user.AccessFailedCount);

            return Result<AuthResultDto>.Failure("Invalid credentials.");
        }

        user.RecordLogin();
        user.AccessFailedCount = 0;

        var roles = await _users.GetRolesAsync(user.Id, ct);
        var tokenSet = _tokens.GenerateToken(user.Id.ToString(), user.UserName ?? user.Email!, roles);

        var refreshTokenExpiry = dto.RememberMe
            ? DateTime.UtcNow.AddDays(_options.RememberMeTokenDays)
            : DateTime.UtcNow.AddDays(_options.RefreshTokenDays);

        await _users.AddRefreshTokenAsync(user.Id, tokenSet.RefreshToken, refreshTokenExpiry, ct);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("User {Email} logged in successfully", dto.Email);

        return Result<AuthResultDto>.Success(
            user.ToResult(roles, tokenSet, refreshTokenExpiry));
    }

    public async Task<Result> LogoutAsync(Guid? userId, CancellationToken ct = default)
    {
        if (userId.HasValue)
        {
            await _users.RevokeRefreshTokenAsync(userId.Value, ct);
            _logger.LogInformation("User {UserId} logged out", userId.Value);
        }

        return Result.Success();
    }

    public async Task<Result<TokenDto>> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken ct = default)
        => await _tokens.RefreshAsync(userId, refreshToken, ct);

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken ct = default)
    {
        if (dto.UserId == Guid.Empty)
            return Result.Failure("Invalid user identifier.");

        var user = await _users.GetByIdAsync(dto.UserId, ct);
        if (user is null)
            return Result.Failure("User not found.");

        if (user.EmailConfirmed)
            return Result.Success();

        if (!await _users.ConfirmEmailAsync(dto.UserId, dto.Token, ct))
            return Result.Failure("Email confirmation failed. The link may be invalid or expired.");

        user.ConfirmEmail();
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Email confirmed for user {UserId}", dto.UserId);

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailDto dto, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(dto.Email.ToUpperInvariant(), ct);
        if (user is null || user.EmailConfirmed)
            return Result.Success();

        var token = await _tokens.GenerateEmailConfirmationTokenAsync(user.Id, ct);
        var link = $"{_options.ConfirmEmailBaseUrl}?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        await _email.SendConfirmationAsync(user.Email!, link, ct);

        _logger.LogInformation("Resent confirmation email to {Email}", dto.Email);

        return Result.Success();
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(dto.Email.ToUpperInvariant(), ct);
        if (user is null)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Success();

        var token = await _tokens.GeneratePasswordResetTokenAsync(user.Id, ct);
        var link = $"{_options.ResetPasswordBaseUrl}?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";
        await _email.SendPasswordResetAsync(user.Email!, link, ct);

        _logger.LogInformation("Password reset link sent to {Email}", dto.Email);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(dto.Email.ToUpperInvariant(), ct);
        if (user is null)
            return Result.Failure("Invalid request.");

        if (!await _users.ResetPasswordAsync(user.Id, dto.Token, dto.NewPassword, ct))
            return Result.Failure("Password reset failed. The link may be invalid or expired.");

        if (user.Status == UserStatus.Locked || user.Status == UserStatus.Suspended)
        {
            user.Unlock();
        }

        await _users.RevokeRefreshTokenAsync(user.Id, ct);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Password reset completed for user {Email}", dto.Email);

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken ct = default)
    {
        if (!await _users.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword, ct))
            return Result.Failure("Current password is incorrect.");

        await _users.RevokeRefreshTokenAsync(userId, ct);

        _logger.LogInformation("Password changed for user {UserId}", userId);

        return Result.Success();
    }

    public async Task<Result> EnableTwoFactorAsync(Guid userId, CancellationToken ct = default)
    {
        if (!await _users.SetTwoFactorEnabledAsync(userId, true, ct))
            return Result.Failure("Unable to enable two-factor authentication.");

        _logger.LogInformation("Two-factor enabled for user {UserId}", userId);

        return Result.Success();
    }
}
