using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using AutoMapper;
using System.Net;
using System.Globalization;

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
    public IActionResult GetPagamentosByFuncionario(int id)
    {
        try
        {
            var pagamentos = _context.Pagamentos
            .Include(p => p.Funcionario)
            .ThenInclude(p => p.AdicionalFuncionario)
            .ThenInclude(ap => ap.Adicional)
            .ThenInclude(a => a.TipoAdicional)
            .Include(p => p.Funcionario.DescontoFuncionario)
            .ThenInclude(dp => dp.Desconto)
            .ThenInclude(a => a.TipoDesconto)
            .Where(p => p.FuncionarioId == id)
            .ToList();

            if (!pagamentos.Any())
            {
                return NotFound($"Nenhum pagamento encontrado para o funcionário com ID {id}");
            }

            // Mapeie os resultados para um formato mais amigável
            var resultado = pagamentos.Select(p => new
            {
                p.Id,
                p.DataPagamento,
                p.Valor,
                Funcionario = new
                {
                    p.Funcionario.Id,
                    p.Funcionario.NomeCompleto,
                    // outras propriedades do funcionário que você deseja incluir
                },
                Adicionais = p.Funcionario.AdicionalFuncionario.Select(af => new
                {
                    af.Adicional.Id,
                    af.Adicional.TipoAdicional.Valor, // ou outras propriedades
                    af.Adicional.ValorFixo
                }),
                Descontos = p.Funcionario.DescontoFuncionario.Select(df => new
                {
                    df.Desconto.Id,
                    df.Desconto.TipoDesconto.Valor, // ou outras propriedades
                    df.Desconto.ValorFixo
                })
            });

            return Ok(resultado);
        }
        catch (Exception)
        {
            _logger.LogError("PagamentoController.GetPagamentosByFuncionario -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GABI}]");
        }
    }

    [HttpGet("gerar-pagamentos")]
    public IActionResult GerarPagamentos()
    {
        var funcionarios = _context.Funcionarios
            .Include(f => f.AdicionalFuncionario)
            .ThenInclude(f => f.Adicional)
            .Include(f => f.DescontoFuncionario)
            .ThenInclude(f => f.Desconto)
            .ToList();

        foreach (var funcionario in funcionarios)
        {
            var pagamento = new Pagamento
            {
                TipoPagamentoCod = 1,
                DataPagamento = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                HorasTrabalhadas = 0,
                Valor = CalcularTotal(funcionario),
                FuncionarioId = funcionario.Id,
            };

            _context.Pagamentos.Add(pagamento);
        }

        _context.SaveChanges();

        return Ok();
    }

    private double CalcularTotal(Funcionario funcionario)
    {
        // Implemente a lógica para calcular o total.
        // Aqui, estou apenas retornando um valor de exemplo.

        double totalAdicionais = funcionario.AdicionalFuncionario?.Sum(af => af.Adicional?.ValorFixo ?? 0) ?? 0;
        double totalDescontos = funcionario.DescontoFuncionario?.Sum(df => df.Desconto?.ValorFixo ?? 0) ?? 0;
        return funcionario.SalarioBase + totalAdicionais - totalDescontos;
    }
}