using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl
{
    public sealed class CreateShortUrlResponse
    {
        public Guid Id { get; init; }

        public string OriginalUrl { get; init; } = default!;

        public string ShortCode { get; init; } = default!;

        public string ShortUrl { get; init; } = default!;

        public int ClickCount { get; init; }
    }
}
