using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UrlShortener.Application.Common.Responses;
using UrlShortener.Application.Features.Urls.Commands.ActivateUrl;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Commands.DeactivateUrl;
using UrlShortener.Application.Features.Urls.Commands.DeleteUrl;
using UrlShortener.Application.Features.Urls.Commands.UpdateUrl;
using UrlShortener.Application.Features.Urls.GetUrls;
using UrlShortener.Application.Features.Urls.Queries.Analytics;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;

namespace UrlShortener.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[EnableRateLimiting("default")]
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

        return Ok(new CreateShortUrlResponse
        {
            Id = dto.Id,
            OriginalUrl = dto.OriginalUrl,
            ShortCode = dto.ShortCode,
            ShortUrl = $"{Request.Scheme}://{Request.Host}/{dto.ShortCode}",
            ClickCount = dto.ClickCount,
            ExpiresOnUtc = dto.ExpiresOnUtc,
            IsActive = dto.IsActive
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetUrls(
     [FromQuery] GetUrlsQuery query,
     CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(
            query,
            cancellationToken));
    }

    //[HttpGet("{id:guid}")]
    //public async Task<IActionResult> GetById(
    //    Guid id,
    //    CancellationToken cancellationToken)
    //{
    //    var response = await _mediator.Send(
    //        new GetUrlQuery(id),
    //        cancellationToken);

    //    return Ok(response);
    //}

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
     Guid id,
     UpdateUrlRequest request,
     CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateUrlCommand(
                id,
                request.OriginalUrl,
                request.ExpirationDateUtc),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Short URL updated successfully."
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteUrlCommand(id),
            cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Short URL deleted successfully."
        });
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ActivateUrlCommand(id),
            cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Short URL activated successfully."
        });
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeactivateUrlCommand(id),
            cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Short URL deactivated successfully."
        });
    }

    [HttpGet("{id:guid}/analytics")]
    public async Task<IActionResult> GetAnalytics(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetUrlAnalyticsQuery(id),
            cancellationToken);

        return Ok(response);
    }
}