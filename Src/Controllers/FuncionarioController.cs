using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using System.Net;

using AutoMapper;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    private readonly ILogger<FuncionarioController> _logger;

    public FuncionarioController(ApplicationDbContext context, IMapper mapper, ILogger<FuncionarioController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // Rota GET: api/funcionario
    [HttpGet]
    [Authorize]
    public IActionResult Get(string? filtro, int? empresa)
    {
        try
        {
            var funcionarios = _context.Funcionario.AsQueryable();

            // implementar a opcao de concatenar filtros ou sempre fazer todos?
            // ignorar acentos?

            if (empresa != null) {
                funcionarios = funcionarios.Where(funcionario => funcionario.Empresa.Id == empresa);
            }

            if (!string.IsNullOrEmpty(filtro))
            {
                string filtroLowerCase = filtro.ToLower();

                funcionarios = funcionarios
                 .Where(funcionario =>
                    funcionario.Nome.ToLower().Contains(filtroLowerCase) ||
                    funcionario.Sobrenome.ToLower().Contains(filtroLowerCase) || //juntar com nome
                    funcionario.Cpf.ToLower().Contains(filtroLowerCase) ||
                    funcionario.Salario_base.ToString().ToLower().Contains(filtroLowerCase) ||
                    funcionario.Jornada_trabalho_semanal.ToString().ToLower().Contains(filtroLowerCase) ||
                    funcionario.TipoCargo.Valor.ToLower().Contains(filtroLowerCase) ||
                    funcionario.Usuario.Email.ToLower().Contains(filtroLowerCase)
                 );
            }

            var result = funcionarios.Select(funcionario => new FuncionarioDto
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Sobrenome = funcionario.Sobrenome,
                Cpf = funcionario.Cpf,
                Cargo = funcionario.TipoCargo.Valor,
                SalarioBase = funcionario.Salario_base,
                JornadaTrabalhoSemanal = funcionario.Jornada_trabalho_semanal,
                Email = funcionario.Usuario.Email,
                Empresa = funcionario.Empresa.Nome
            }).ToList();
            
            _logger.LogInformation($"FuncionarioController.Get -> [Success]");
            return Ok(result);
        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GF}]");
        }
    }

    // POST: api/funcionario
    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateFuncionarioRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Usuario.FirstOrDefault(u => u.Email == request.Usuario_email);

            if (user == null) {
                throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CF}]");
            }

            Funcionario funcionario = _mapper.Map<Funcionario>(request);
            funcionario.Usuario_id = user.Id;

            _context.Funcionario.Add(funcionario);
            _context.SaveChanges();
            
            _logger.LogInformation("FuncionarioController.Create -> [Success]");
            return Created("", new { message = $"Funcionario -> {funcionario.Nome} criado com sucesso" });
        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CF}]");
        }
    }
}