using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using AutoMapper;
using System.Net;
using System.IdentityModel.Tokens.Jwt;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(ApplicationDbContext context, IMapper mapper, ILogger<UsuarioController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // Rota GET: api/usuario
    // params: filtro [string][optional] (filtra usuario por email)
    [HttpGet]
    [Authorize]
    public IActionResult Get(string filtro = "")
    {
        try
        {
            var usuarios = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                usuarios = usuarios
                 .Where(u => u.Email.ToLower().Contains(filtro.ToLower()));
            }

            List<UsuarioDto> result = usuarios.Select(u => new UsuarioDto
            {
                Email = u.Email,
                Tipo = u.TipoUsuario.Valor
            }).ToList();

            _logger.LogInformation("UsuarioController.Get -> [Success]");
            return Ok(result);
        }
        catch (Exception)
        {
            _logger.LogError("UsuarioController.Get -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GU}]");
        }
    }

    // POST: api/usuario
    [HttpPost]
    public IActionResult Create([FromBody] CreateUsuarioRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Usuario usuario = _mapper.Map<Usuario>(request);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            _logger.LogInformation("UsuarioController.Create -> [Success]");
            return Created("", new { message = "usuario criado com sucesso" });
        }
        catch (Exception)
        {
            _logger.LogError("UsuarioController.Create -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.CU}]");
        }
    }

    // DELETE: api/usuario/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        try
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            _logger.LogInformation("UsuarioController.Delete -> [Success]");
            return Ok("Usuário excluído com sucesso.");
        }
        catch (Exception)
        {
            _logger.LogError("UsuarioController.Delete -> [Error]");
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.DU}]");
        }
    }

    [HttpPost("trocar-senha")]
    [Authorize]
    public async Task<IActionResult> TrocarSenha([FromBody] TrocaSenhaDto trocaSenhaDto)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Valida e decodifica o token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            // Obtém o valor da propriedade 'sub' (ID do usuário)
            var usuarioId = jsonToken?.Payload["sub"].ToString();

            if (usuarioId == null) {
                return Unauthorized();
            }

            int usuarioIdInt = int.Parse(usuarioId);

        var usuario = await _context.Usuarios.FindAsync(usuarioIdInt);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado");
        }

        // Lógica para verificar a senha antiga antes de permitir a troca de senha
        if (usuario.Senha != trocaSenhaDto.SenhaAntiga)
        {
            return BadRequest("Senha antiga incorreta");
        }

        // Atualize a senha do usuário
        usuario.Senha = trocaSenhaDto.NovaSenha;

        // Salve as mudanças no banco de dados
        await _context.SaveChangesAsync();

        return Ok("Senha trocada com sucesso");
    }
}

