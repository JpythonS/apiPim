using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api_pim.Entities;
using api_pim.Models;

using AutoMapper;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    public FuncionarioController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Rota GET: api/funcionario
    [HttpGet]
    [Authorize]
    public IActionResult Get(string filtro = "")
    {
        var funcionarios = _context.Funcionario.AsQueryable();

        // implementar a opcao de concatenar filtros ou sempre fazer todos?
        // ignorar acentos?

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
            Email = funcionario.Usuario.Email
        }).ToList();

        return Ok(result);
    }

    // POST: api/funcionario
    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateFuncionarioRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        Funcionario funcionario = _mapper.Map<Funcionario>(request);

        _context.Funcionario.Add(funcionario);
        _context.SaveChanges();

        return Created("", new { message = $"Funcionario -> {funcionario.Nome} criado com sucesso" });
    }
}