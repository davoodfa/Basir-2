using System.Text.RegularExpressions;
using Basir.Domain.Common;

namespace Basir.Domain.ValueObjects;

public sealed partial class Email : ValueObject
{
    private static readonly Regex EmailRegex = EmailRegexPattern();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EmailRegexPattern();

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<Email>.Failure("Email cannot be empty.");

        if (!EmailRegex.IsMatch(email))
            return Result<Email>.Failure("Invalid email format.");

        return Result<Email>.Success(new Email(email.Trim().ToLowerInvariant()));
    }

    public string NormalizedValue => Value.ToUpperInvariant();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
