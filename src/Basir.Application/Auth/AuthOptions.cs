namespace Basir.Application.Auth;

public class AuthOptions
{
    public const string SectionName = "Auth";
    public string ConfirmEmailBaseUrl { get; set; } = string.Empty;
    public string ResetPasswordBaseUrl { get; set; } = string.Empty;
    public int MaxFailedAccessAttempts { get; set; } = 5;
    public int LockoutMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;
    public int RememberMeTokenDays { get; set; } = 30;
}
