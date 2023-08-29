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
public class DescontoController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<EmpresaController> _logger;

    private readonly ApplicationDbContext _context;

    public DescontoController(IMapper mapper, ILogger<EmpresaController> logger, ApplicationDbContext context)
    {
        _mapper = mapper;
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var descontos = _context.Descontos.ToList();
        return Ok(descontos);
    }

    [HttpGet("{id}")]
    public IActionResult GetByFuncionarioId(int id)
    {
        try
        {
            var funcionario = _context.Funcionarios
            .Include(f => f.DescontoFuncionario)
            .ThenInclude(f => f.Desconto)
            .FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            var descontos = funcionario.DescontoFuncionario.Select(fa => fa.Desconto);

            return Ok(descontos);

        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GABI}]");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateDescontoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Desconto desconto = _mapper.Map<Desconto>(request);

            _context.Descontos.Add(desconto);
            _context.SaveChanges();
            _logger.LogInformation("DescontoController.Create -> [Success]");
            return Created("", new { message = $"Desconto criado com sucesso" });
        }
        catch (Exception)
        {
            _logger.LogError("DescontoController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CA}]");
        }
    }
}