using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Users.Commands.LoginUser;
using UrlShortener.Application.Features.Users.Commands.RegisterUser;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(Register),
            new { id },
            new
            {
                Id = id,
                Message = "User registered successfully."
            });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }
}