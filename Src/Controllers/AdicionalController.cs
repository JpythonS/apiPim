using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using AutoMapper;
using System.Net;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdicionalController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<EmpresaController> _logger;

    private readonly ApplicationDbContext _context;

    public AdicionalController(IMapper mapper, ILogger<EmpresaController> logger, ApplicationDbContext context)
    {
        _mapper = mapper;
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var adicionais = _context.Adicionais.ToList();
        return Ok(adicionais);
    }

    [HttpGet("{id}")]
    public IActionResult GetByFuncionarioId(int id)
    {
        try
        {
            var funcionario = _context.Funcionarios
            .Include(f => f.AdicionalFuncionario)
            .ThenInclude(f => f.Adicional)
            .FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            var adicionais = funcionario.AdicionalFuncionario.Select(fa => fa.Adicional);

            return Ok(adicionais);

        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GABI}]");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateAdicionalRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Adicional adicional = _mapper.Map<Adicional>(request);

            _context.Adicionais.Add(adicional);
            _context.SaveChanges();
            _logger.LogInformation("AdicionalController.Create -> [Success]");
            return Created("", new { message = $"Adicional criado com sucesso" });
        }
        catch (Exception)
        {
            _logger.LogError("AdicionalController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CA}]");
        }
    }
}