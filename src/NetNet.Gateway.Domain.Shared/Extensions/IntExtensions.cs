namespace NetNet.Gateway.Extensions;

public static class IntExtensions
{
    public static TimeSpan? ToTimeSpan(this int? seconds)
    {
        if (seconds.HasValue)
        {
            return TimeSpan.FromSeconds(seconds.Value);
        }

        return default;
    }
}
