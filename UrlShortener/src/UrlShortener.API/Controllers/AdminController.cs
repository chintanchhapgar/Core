using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Common.Responses;
using UrlShortener.Application.Features.Admin.ActivateUrl;
using UrlShortener.Application.Features.Admin.Dashboard;
using UrlShortener.Application.Features.Admin.DeactivateUrl;
using UrlShortener.Application.Features.Admin.DeleteUrl;
using UrlShortener.Application.Features.Admin.GetAllUrls;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public sealed class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetDashboardQuery(),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("urls")]
    public async Task<IActionResult> GetUrls(
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetAllUrlsQuery(),
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("urls/{id:guid}/activate")]
    public async Task<IActionResult> Activate(
     Guid id,
     CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ActivateUrlCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Short URL activated successfully."
        });
    }

    [HttpPut("urls/{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeactivateUrlCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Short URL deactivated successfully."
        });
    }

    [HttpDelete("urls/{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteUrlCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Short URL deleted successfully."
        });
    }
}