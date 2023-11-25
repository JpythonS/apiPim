using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using System.Net;

using AutoMapper;
using System.Numerics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

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

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteById(int id)
    {
        try
        {
            var funcionario = _context.Funcionarios.Find(id);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            _context.Funcionarios.Remove(funcionario);
            _context.SaveChanges();

            return Ok($"Funcionário com ID {id} excluído com sucesso.");
        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GF}]");
        }
    }

    // Rota GET: api/funcionario
    [HttpGet]
    [Authorize]
    public IActionResult Get(string? filtro, int? empresa, int? id)
    {
        try
        {
            var funcionarios = _context.Funcionarios.AsQueryable();

            // implementar a opcao de concatenar filtros ou sempre fazer todos?
            // ignorar acentos?


            if (id != null)
            {
                funcionarios = funcionarios.Where(funcionario => funcionario.Id == id);
            }

            if (empresa != null)
            {
                funcionarios = funcionarios.Where(funcionario => funcionario.Empresa.Id == empresa);
            }

            if (!string.IsNullOrEmpty(filtro))
            {
                string filtroLowerCase = filtro.ToLower();

                funcionarios = funcionarios
                 .Where(funcionario =>
                    funcionario.NomeCompleto.ToLower().Contains(filtroLowerCase) ||
                    funcionario.Endereco.ToLower().Contains(filtroLowerCase) ||
                    funcionario.Cpf.ToLower().Contains(filtroLowerCase) ||
                    funcionario.SalarioBase.ToString().ToLower().Contains(filtroLowerCase) ||
                    funcionario.JornadaTrabalhoSemanal.ToString().ToLower().Contains(filtroLowerCase) ||
                    funcionario.TipoCargo.Valor.ToLower().Contains(filtroLowerCase) ||
                    funcionario.Usuario.Email.ToLower().Contains(filtroLowerCase)
                 );
            }

            var result = funcionarios.Select(funcionario => new FuncionarioDto
            {
                Id = funcionario.Id,
                Nome = funcionario.NomeCompleto,
                DataNascimento = funcionario.DataNascimento,
                Cpf = funcionario.Cpf,
                Rg = funcionario.Rg,
                Celular = funcionario.Celular,
                CelularContatoEmergencia = funcionario.CelularContatoEmergencia,
                Bairro = funcionario.Bairro,
                Cidade = funcionario.Cidade,
                Estado = funcionario.Estado,
                Pis = funcionario.Pis,
                Cargo = funcionario.TipoCargo.Valor,
                Endereco = funcionario.Endereco,
                SalarioBase = funcionario.SalarioBase,
                JornadaTrabalhoSemanal = funcionario.JornadaTrabalhoSemanal,
                Email = funcionario.Usuario.Email,
                Empresa = funcionario.Empresa.Nome,
                NivelPermissao = funcionario.Usuario.TipoUsuario.Valor
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

    // Rota GET: api/funcionario
    [HttpGet("perfil")]
    [Authorize]
    public IActionResult GetByTokenId()
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Valida e decodifica o token
            JwtSecurityTokenHandler tokenHandler = new();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;


            // Obtém o valor da propriedade 'sub' (ID do usuário)
            var usuarioId = jsonToken?.Payload["sub"].ToString();

            if (usuarioId == null)
            {
                return Unauthorized();
            }

            int usuarioIdInt = int.Parse(usuarioId);

            var result = _context.Funcionarios.AsQueryable()
            .Where(funcionario => funcionario.Usuario.Id == usuarioIdInt)
            .Select(funcionario => new FuncionarioDto
            {
                Id = funcionario.Id,
                Nome = funcionario.NomeCompleto,
                DataNascimento = funcionario.DataNascimento,
                Cpf = funcionario.Cpf,
                Rg = funcionario.Rg,
                Celular = funcionario.Celular,
                CelularContatoEmergencia = funcionario.CelularContatoEmergencia,
                Bairro = funcionario.Bairro,
                Cidade = funcionario.Cidade,
                Estado = funcionario.Estado,
                Pis = funcionario.Pis,
                Cargo = funcionario.TipoCargo.Valor,
                Endereco = funcionario.Endereco,
                SalarioBase = funcionario.SalarioBase,
                JornadaTrabalhoSemanal = funcionario.JornadaTrabalhoSemanal,
                Email = funcionario.Usuario.Email,
                Empresa = funcionario.Empresa.Nome,
                NivelPermissao = funcionario.Usuario.TipoUsuario.Valor
            }).First();

            _logger.LogInformation($"FuncionarioController.GetByTokenId -> [Success]");
            return Ok(result);
        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.GetByTokenId -> [Error]");
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


            CreateUsuarioRequest createUsuarioRequest = new()
            {
                Email = request.UsuarioEmail,
                Senha = request.Cpf,
                TipoUsuarioCod = 1
            };

            Usuario usuario = _mapper.Map<Usuario>(createUsuarioRequest);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var user = _context.Usuarios.FirstOrDefault(u => u.Email == request.UsuarioEmail);

            if (user == null)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, $"Usuario não encontrado [{ErrorCode.CF}]");
            }

            Funcionario funcionario = _mapper.Map<Funcionario>(request);
            funcionario.UsuarioId = user.Id;

            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();

            _logger.LogInformation("FuncionarioController.Create -> [Success]");
            return Created("", new { message = $"Funcionario -> {funcionario.NomeCompleto} criado com sucesso" });
        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CF}]");
        }
    }

    [HttpPost("atualizar/{id}")]
    [Authorize]
    public IActionResult Update(int id, [FromBody] UpdateFuncionarioRequest funcionarioAtualizado)
    {
        var funcionarioExistente = _context.Funcionarios.Find(id);
        if (funcionarioExistente == null) return NotFound("Funcionário não encontrado.");

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Cargo))
        {
            var cargo = _context.TipoCargo.Find(int.Parse(funcionarioAtualizado.Cargo));
            if (cargo == null) return NotFound("Cargo não encontrado.");

            funcionarioExistente.TipoCargo = cargo;
        }

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Empresa))
        {
            var empresa = _context.Empresas.Find(int.Parse(funcionarioAtualizado.Empresa));
            if (empresa == null) return NotFound("Empresa não encontrada.");

            funcionarioExistente.Empresa = empresa;
        }

        // Aqui, você pode atualizar apenas as propriedades que foram fornecidas no objeto funcionarioAtualizado.

        if (!string.IsNullOrEmpty(funcionarioAtualizado.NomeCompleto))
            funcionarioExistente.NomeCompleto = funcionarioAtualizado.NomeCompleto;

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Endereco))
            funcionarioExistente.Endereco = funcionarioAtualizado.Endereco;

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Celular))
            funcionarioExistente.Celular = funcionarioAtualizado.Celular;

        if (!string.IsNullOrEmpty(funcionarioAtualizado.CelularContatoEmergencia))
            funcionarioExistente.CelularContatoEmergencia = funcionarioAtualizado.CelularContatoEmergencia;

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Bairro))
            funcionarioExistente.Bairro = funcionarioAtualizado.Bairro;

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Cidade))
            funcionarioExistente.Cidade = funcionarioAtualizado.Cidade;

        if (!string.IsNullOrEmpty(funcionarioAtualizado.Estado))
            funcionarioExistente.Estado = funcionarioAtualizado.Estado;


        if (!string.IsNullOrEmpty(funcionarioAtualizado.SalarioBase))
            funcionarioExistente.SalarioBase = double.Parse(funcionarioAtualizado.SalarioBase);


        if (!string.IsNullOrEmpty(funcionarioAtualizado.JornadaTrabalhoSemanal)) funcionarioExistente.JornadaTrabalhoSemanal = double.Parse(funcionarioAtualizado.JornadaTrabalhoSemanal);

        _context.SaveChanges();

        return Ok("Funcionário atualizado com sucesso.");
    }

    [HttpPost("adicionais")]
    public IActionResult VincularAdicional([FromBody]   VincularAdicionalRequest adicional)
    {
        try
        {
            // Encontrar o funcionário pelo número da matrícula
            var funcionario = _context.Funcionarios.Find(adicional.FuncionarioId);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com matrícula {adicional.FuncionarioId} não encontrado.");
            }
            AdicionalFuncionario adicionalFuncionario = new()
            {
                FuncionarioId = adicional.FuncionarioId,
                AdicionalId = adicional.AdicionalId

            };
            // Adicionar o adicional ao funcionário
            funcionario.AdicionalFuncionario.Add(adicionalFuncionario);

            _context.SaveChanges();

            return Ok($"Adicional adicionado com sucesso ao funcionário {adicional.FuncionarioId}.");
        }
        catch (Exception)
        {
            _logger.LogError("FuncionarioController.VincularAdicional -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CF}]");
        }
    }


}