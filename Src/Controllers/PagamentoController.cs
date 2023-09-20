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
public class PagamentoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    private readonly ILogger<PagamentoController> _logger;

    public PagamentoController(ApplicationDbContext context, IMapper mapper, ILogger<PagamentoController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public IActionResult GetByFuncionarioId(int id)
    {
        try
        {
            var funcionario = _context.Funcionarios
            .Include(f => f.AdicionalFuncionario)
            .ThenInclude(f => f.Adicional)
            .Include(f => f.DescontoFuncionario)
            .ThenInclude(f => f.Desconto)
            .FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }


            return Ok(funcionario);

        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GABI}]");
        }
    }
}