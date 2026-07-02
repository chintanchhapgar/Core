public sealed record UpdateUrlRequest(
    string OriginalUrl,
    DateTime? ExpirationDateUtc);