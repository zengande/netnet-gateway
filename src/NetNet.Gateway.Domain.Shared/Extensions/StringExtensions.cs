namespace NetNet.Gateway.Extensions;

public static class StringExtensions
{
    public static IEnumerable<string>? SplitAs(this string? str, string separator,
        StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
    {
        if (str is null) return default;

        return str.Split(separator, splitOptions);
    }
}
