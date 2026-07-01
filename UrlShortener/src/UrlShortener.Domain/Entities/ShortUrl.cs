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

    private ShortUrl()
    {
    }

    public ShortUrl(string originalUrl, string shortCode)
    {
        OriginalUrl = originalUrl;
        ShortCode = shortCode;
    }

    public void RegisterClick()
    {
        ClickCount++;
    }
}
