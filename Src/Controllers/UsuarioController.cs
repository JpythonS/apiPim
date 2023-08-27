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
}
