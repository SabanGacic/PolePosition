using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolePosition.API.DTOs;
using PolePosition.API.Services;

namespace PolePosition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        => Ok(await _authService.RegisterAsync(dto));

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        => Ok(await _authService.LoginAsync(dto));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return NoContent();
    }
}