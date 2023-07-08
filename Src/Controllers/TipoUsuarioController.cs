using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;
using System.Net;
using api_pim.Exceptions;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoUsuarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<TipoUsuarioController> _logger;

    public TipoUsuarioController(ApplicationDbContext context, ILogger<TipoUsuarioController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TipoUsuario>>> Get()
    {
        try
        {
            var result = await _context.TipoUsuario.ToListAsync();
            _logger.LogInformation("TipoUsuarioController.Get -> [Success]");
            return result;
        }
        catch (Exception)
        {
            _logger.LogError("TipoUsuarioController.Get -> [Error]");
           throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CTU}]");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] TipoUsuario request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Dados do tipo de usuário inválidos." });
            }

            _context.TipoUsuario.Add(request);
            _context.SaveChanges();
            _logger.LogInformation("TipoUsuarioController.Get -> [Success]");
            
            return Ok($"Tipo de usuario [{request.Valor}] criado com sucesso");
        }
        catch (Exception)
        {
            _logger.LogError("TipoUsuarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GTU}]");
        }
    }
}