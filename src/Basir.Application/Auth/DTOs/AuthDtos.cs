namespace Basir.Application.Auth.DTOs;

public record LoginDto(string Email, string Password, bool RememberMe);

public record RegisterDto(
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber);

public record ForgotPasswordDto(string Email);

public record ResetPasswordDto(string Email, string Token, string NewPassword, string ConfirmPassword);

public record ChangePasswordDto(string CurrentPassword, string NewPassword, string ConfirmPassword);

public record ConfirmEmailDto(Guid UserId, string Token);

public record ResendConfirmationEmailDto(string Email);

public record TokenDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);

public record AuthResultDto(
    Guid UserId,
    string UserName,
    string Email,
    IReadOnlyList<string> Roles,
    TokenDto? Tokens,
    DateTime? RefreshTokenExpiresAt = null);

public record CurrentUserDto(Guid UserId, string UserName, string Email, IReadOnlyList<string> Roles);
