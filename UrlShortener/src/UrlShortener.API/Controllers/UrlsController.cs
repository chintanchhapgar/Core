using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Urls.Analytics;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.GetUrls;

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

    [Authorize]
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

    [Authorize]
    [HttpGet("{id:guid}/analytics")]
    public async Task<IActionResult> GetAnalytics(Guid id)
    {
        var response = await _mediator.Send(
            new GetUrlAnalyticsQuery(id));

        return Ok(response);
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUrls(
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetUrlsQuery(),
            cancellationToken);

        return Ok(response);
    }
}