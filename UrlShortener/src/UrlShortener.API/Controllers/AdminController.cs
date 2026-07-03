using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Common.Responses;
using UrlShortener.Application.Features.Admin.Dashboard;
using UrlShortener.Application.Features.Admin.Users.ActivateUser;
using UrlShortener.Application.Features.Admin.Users.DeactivateUser;
using UrlShortener.Application.Features.Admin.Users.GetUser;
using UrlShortener.Application.Features.Admin.Users.GetUsers;
using UrlShortener.Application.Features.Admin.Users.LockUser;
using UrlShortener.Application.Features.Admin.Users.UnlockUser;
using UrlShortener.Application.Features.Admin.Users.UpdateRole;
using UrlShortener.Domain.Constants;

[Authorize(Roles = Roles.Admin)]
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

    [HttpPut("users/{id:guid}/role")]
    public async Task<IActionResult> UpdateUserRole(
    Guid id,
    UpdateUserRoleRequest request,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateUserRoleCommand(
                id,
                request.Role),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User role updated successfully."
        });
    }

    [HttpPut("users/{id:guid}/lock")]
    public async Task<IActionResult> LockUser(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new LockUserCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User locked successfully."
        });
    }

    [HttpPut("users/{id:guid}/unlock")]
    public async Task<IActionResult> UnlockUser(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UnlockUserCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User unlocked successfully."
        });
    }

    [HttpPut("users/{id:guid}/activate")]
    public async Task<IActionResult> ActivateUser(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ActivateUserCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User activated successfully."
        });
    }

    [HttpPut("users/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeactivateUserCommand(id),
            cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User deactivated successfully."
        });
    }
}