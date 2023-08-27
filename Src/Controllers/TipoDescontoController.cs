using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;
using System.Net;
using api_pim.Exceptions;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoDescontoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<TipoDescontoController> _logger;

    public TipoDescontoController(ApplicationDbContext context, ILogger<TipoDescontoController> logger)
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
            var result = _context.TipoDesconto.ToList();
            _logger.LogInformation("TipoDescontoController.Get -> [Success]");
            return Ok(result);
        }
        catch (Exception)
        {
            _logger.LogError("TipoDescontoController.Get -> [Error]");
           throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GTD}]");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] TipoDesconto request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Dados do tipo de desconto inválidos." });
            }

            _context.TipoDesconto.Add(request);
            _context.SaveChanges();
            _logger.LogInformation("TipoDescontoController.Get -> [Success]");
            
            return Ok($"Tipo de desconto [{request.Valor}] criado com sucesso");
        }
        catch (Exception)
        {
            _logger.LogError("TipoDescontoController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CTD}]");
        }
    }

    [HttpDelete]
    [Authorize]
    public IActionResult Delete(int cod) {
        try
        {
            var tipoDesconto = _context.TipoDesconto.FirstOrDefault(td => td.Cod == cod);
            if (tipoDesconto == null) {
                return NotFound(new { message = "Tipo de desconto não encontrado" });
            }

            _context.TipoDesconto.Remove(tipoDesconto);
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            _logger.LogError("TipoDescontoController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.DTD}]");
        }
    }
}