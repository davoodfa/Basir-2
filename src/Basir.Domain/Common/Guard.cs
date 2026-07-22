namespace Basir.Domain.Common;

public static class Guard
{
    public static T NotNull<T>(T? value, string name) where T : class
    {
        if (value is null)
            throw new ArgumentNullException(name);
        return value;
    }

    public static string NotNullOrWhiteSpace(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} cannot be null or whitespace.", name);
        return value;
    }
}
