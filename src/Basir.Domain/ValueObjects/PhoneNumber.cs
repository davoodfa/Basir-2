using System.Text.RegularExpressions;
using Basir.Domain.Common;

namespace Basir.Domain.ValueObjects;

public sealed partial class PhoneNumber : ValueObject
{
    private static readonly Regex PhoneRegex = PhoneRegexPattern();

    [GeneratedRegex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled)]
    private static partial Regex PhoneRegexPattern();

    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result<PhoneNumber>.Failure("Phone number cannot be empty.");

        var cleaned = phoneNumber
            .Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        if (!PhoneRegex.IsMatch(cleaned))
            return Result<PhoneNumber>.Failure("Invalid phone number format. Must be in E.164 format.");

        return Result<PhoneNumber>.Success(new PhoneNumber(cleaned));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
