public sealed record ResolveShortUrlResponse(
    string OriginalUrl,
    bool IsExpired,
    bool IsDisabled);