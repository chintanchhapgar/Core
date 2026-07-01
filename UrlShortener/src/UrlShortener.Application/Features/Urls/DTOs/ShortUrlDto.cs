using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.Urls.DTOs;

public sealed class ShortUrlDto
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortCode { get; init; } = string.Empty;

    public int ClickCount { get; init; }
}