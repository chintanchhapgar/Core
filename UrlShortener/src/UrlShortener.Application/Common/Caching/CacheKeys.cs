public static class CacheKeys
{
    public static string Url(string shortCode)
        => $"url:{shortCode.ToLowerInvariant()}";

    public static string UrlDetails(Guid id)
        => $"url-details:{id}";

    public static string Analytics(Guid id)
        => $"analytics:{id}";

    public static string User(Guid id)
        => $"user:{id}";

    public const string Dashboard = "dashboard";
}