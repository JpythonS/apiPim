using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using AutoMapper;
using System.Net;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpresaController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<EmpresaController> _logger;

    private readonly ApplicationDbContext _context;

    public EmpresaController(IMapper mapper, ILogger<EmpresaController> logger, ApplicationDbContext context)
    {
        _mapper = mapper;
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        var empresa = _context.Empresas.ToList();
        return Ok(empresa);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateEmpresaRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Empresa empresa = _mapper.Map<Empresa>(request);

            _context.Empresas.Add(empresa);
            _context.SaveChanges();
            _logger.LogInformation("EmpresaController.Create -> [Success]");
            return Created("", new { message = $"Empresa [{request.Nome}] criada com sucesso" });
        }
        catch (Exception)
        {
            _logger.LogError("EmpresaController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CE}]");
        }
    }


    [HttpGet("relatorio-funcionarios")]
    public IActionResult ObterMediaSalarialPorEmpresa()
    {
        var resultado = _context.Empresas
            .Select(e => new MediaSalarialPorEmpresaDto
            {
                NomeEmpresa = e.Nome, // Substitua pelo nome da propriedade correta em sua classe Empresa
                MediaSalarial = e.Funcionarios.Average(f => f.SalarioBase), // Média salarial
                SomaSalarial = e.Funcionarios.Sum(f => f.SalarioBase), // Soma dos salários
                QuantidadeFuncionarios = e.Funcionarios.Count() // Contagem de funcionários
            })
            .ToList();

        return Ok(resultado);
    }
}

public class MediaSalarialPorEmpresaDto
{
    public string NomeEmpresa { get; set; } = string.Empty;
    public double? MediaSalarial { get; set; }
    public double? SomaSalarial { get; set; }
    public int? QuantidadeFuncionarios { get; set; }
}