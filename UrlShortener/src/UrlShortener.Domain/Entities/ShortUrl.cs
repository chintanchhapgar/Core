using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.Entities;

public class ShortUrl : AuditableEntity
{
    public string OriginalUrl { get; private set; } = string.Empty;

    public string ShortCode { get; private set; } = string.Empty;

    public int ClickCount { get; private set; }

    public DateTime? ExpiresOnUtc { get; private set; }
    public Guid? UserId { get; private set; }

    public bool IsActive { get; private set; } = true;
    private ShortUrl()
    {
    }

    public ShortUrl(
    string originalUrl,
    string shortCode,
    Guid? userId = null)
    {
        Id = Guid.NewGuid();
        OriginalUrl = originalUrl;
        ShortCode = shortCode;
        UserId = userId;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void RegisterClick()
    {
        ClickCount++;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public bool IsExpired()
    {
        return ExpiresOnUtc.HasValue &&
               ExpiresOnUtc.Value <= DateTime.UtcNow;
    }

    public void SetExpiration(DateTime? expiresOnUtc)
    {
        ExpiresOnUtc = expiresOnUtc;
    }
}
