using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UrlsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateShortUrlCommand command)
    {
        var dto = await _mediator.Send(command);

        var response = new CreateShortUrlResponse
        {
            Id = dto.Id,
            OriginalUrl = dto.OriginalUrl,
            ShortCode = dto.ShortCode,
            ShortUrl = $"{Request.Scheme}://{Request.Host}/{dto.ShortCode}",
            ClickCount = dto.ClickCount
        };

        return Ok(response);
    }
}