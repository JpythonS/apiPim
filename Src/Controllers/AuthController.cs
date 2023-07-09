using Microsoft.AspNetCore.Mvc;

using api_pim.Interfaces;
using api_pim.Models;
using api_pim.Exceptions;
using System.Net;
using Microsoft.Extensions.Logging;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = _authService.Authenticate(request.Email!, request.Senha!);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("AuthController.Login -> [Error]");
                return Unauthorized(new { message = "token invalido" });
            }

            _logger.LogInformation("AuthController.Login -> [Success]");
            return Ok(new TokenResponse { Token = token });
        }
        catch (Exception)
        {   
            _logger.LogError("AuthController.Login -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CU}]");
        }
    }
}