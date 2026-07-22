namespace Basir.Domain.Enums;

public enum UserStatus
{
    Pending = 0,
    Active = 1,
    Locked = 2,
    Suspended = 3
}

public enum AuthProvider
{
    Local = 0,
    Google = 1,
    Microsoft = 2
}

public enum TokenType
{
    EmailConfirmation = 0,
    PasswordReset = 1,
    PhoneConfirmation = 2,
    Refresh = 3,
    TwoFactor = 4
}
