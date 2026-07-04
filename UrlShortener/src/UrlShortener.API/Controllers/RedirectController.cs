using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UrlShortener.Application.Features.Urls.Commands.ResolveShortUrl;

namespace UrlShortener.API.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IMediator _mediator;

    public RedirectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableRateLimiting("redirect")]
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var url = await _mediator.Send(new ResolveShortUrlCommand(shortCode));

        if (url is null)
            return NotFound();

        return Redirect(url);
    }


}