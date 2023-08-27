using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;
using System.Net;
using api_pim.Exceptions;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoAdicionalController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<TipoAdicionalController> _logger;

    public TipoAdicionalController(ApplicationDbContext context, ILogger<TipoAdicionalController> logger)
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
            var result = _context.TipoAdicional.ToList();
            _logger.LogInformation("TipoAdicionalController.Get -> [Success]");
            return Ok(result);
        }
        catch (Exception)
        {
            _logger.LogError("TipoAdicionalController.Get -> [Error]");
           throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GTA}]");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] TipoAdicional request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Dados do tipo de adicional inválidos." });
            }

            _context.TipoAdicional.Add(request);
            _context.SaveChanges();
            _logger.LogInformation("TipoAdicionalController.Get -> [Success]");
            
            return Ok($"Tipo de cargo [{request.Valor}] criado com sucesso");
        }
        catch (Exception)
        {
            _logger.LogError("TipoAdicionalController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CTC}]");
        }
    }

    [HttpDelete]
    [Authorize]
    public IActionResult Delete(int cod) {
        try
        {
            var tipoAdicional = _context.TipoAdicional.FirstOrDefault(x => x.Cod == cod);
            if (tipoAdicional == null) {
                return NotFound(new { message = "Tipo de adicional não encontrado" });
            }

            _context.TipoAdicional.Remove(tipoAdicional);
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            _logger.LogError("TipoAdicionalController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.DTA}]");
        }
    }
}