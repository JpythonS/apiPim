using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;
using System.Net;
using api_pim.Exceptions;

namespace api_pim.Controllers;

[ApiController]
[Route("api/tipo-usuario")]
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
    public IActionResult Get()
    {
        try
        {
            var result = _context.TipoUsuario.ToList();
            _logger.LogInformation("TipoUsuarioController.Get -> [Success]");
            return Ok(result);
        }
        catch (Exception)
        {
            _logger.LogError("TipoUsuarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GTU}]");
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

            return Ok($"Tipo de usuário [{request.Valor}] criado com sucesso");
        }
        catch (Exception)
        {
            _logger.LogError("TipoUsuarioController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CTU}]");
        }
    }

    [HttpDelete]
    [Authorize]
    public IActionResult Delete(int cod)
    {
        try
        {
            var tipoUsuario = _context.TipoUsuario.FirstOrDefault(td => td.Cod == cod);
            if (tipoUsuario == null)
            {
                return NotFound(new { message = "Tipo de usuário não encontrado" });
            }

            _context.TipoUsuario.Remove(tipoUsuario);
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            _logger.LogError("TipoUsuarioController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.DTD}]");
        }
    }
}