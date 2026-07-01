using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Urls.Queries.Redirect;

namespace UrlShortener.API.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IMediator _mediator;

    public RedirectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var url = await _mediator.Send(new GetOriginalUrlQuery(shortCode));

        if (url is null)
            return NotFound();

        return Redirect(url);
    }
}