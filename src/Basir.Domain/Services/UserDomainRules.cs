using Basir.Domain.Enums;

namespace Basir.Domain.Services;

public static class UserDomainRules
{
    public static class Password
    {
        public const int MinimumLength = 12;
        public const int MaximumLength = 128;

        public const string UppercasePattern = "[A-Z]";
        public const string LowercasePattern = "[a-z]";
        public const string DigitPattern = "[0-9]";
        public const string SpecialCharPattern = "[^a-zA-Z0-9]";
    }

    public static bool IsAccountLocked(UserStatus status, DateTimeOffset? lockoutEnd)
    {
        return status is UserStatus.Locked or UserStatus.Suspended
            || (lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow);
    }
}
