using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using UrlShortener.Application.Features.Urls.DTOs;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed record CreateShortUrlCommand(
    string OriginalUrl)
    : IRequest<ShortUrlDto>;
