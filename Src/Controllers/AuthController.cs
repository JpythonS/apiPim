using Microsoft.AspNetCore.Mvc;

using api_pim.Interfaces;
using api_pim.Models;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService = null!;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string token = _authService.Authenticate(request.Email!, request.Senha!);

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "token invalido" });
        }

        return Ok(new TokenResponse { Token = token });
    }
}