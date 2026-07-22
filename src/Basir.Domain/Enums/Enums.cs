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

public enum ThemeType
{
    Light = 0,
    Dark = 1,
    Custom = 2
}

public enum Direction
{
    Ltr = 0,
    Rtl = 1
}

public enum CalendarType
{
    Gregorian = 0,
    Persian = 1
}

public enum NumberFormatType
{
    Western = 0,
    EasternArabic = 1
}
