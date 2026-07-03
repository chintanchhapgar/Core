using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Admin.Dashboard;
using UrlShortener.Application.Features.Admin.Users.GetUser;
using UrlShortener.Application.Features.Admin.Users.GetUsers;

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

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
     CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetUsersQuery(),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("users/{id:guid}")]
    public async Task<IActionResult> GetUser(
      Guid id,
      CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetUserQuery(id),
            cancellationToken);

        return Ok(response);
    }

}