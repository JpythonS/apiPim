using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Interfaces;

using AutoMapper;
namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IUsuarioService _usuarioService;

    private readonly IMapper _mapper;

    public UsuarioController(ApplicationDbContext context, IUsuarioService usuarioService, IMapper mapper)
    {
        _context = context;
        _usuarioService = usuarioService;
        _mapper = mapper;
    }

    // Rota GET: api/usuarios
    // params: filtro [string][optional] (filtra usuario por email)
    [HttpGet]
    [Authorize]
    public IActionResult Get(string filtro = "")
    {
        var usuarios = _context.Usuario.AsQueryable();

        if (!string.IsNullOrEmpty(filtro))
        {
            usuarios = usuarios
             .Where(u => u.Email.ToLower().Contains(filtro.ToLower()));
        }

        List<UsuarioDto> result = usuarios.Select(u => new UsuarioDto
        {
            Email = u.Email,
            Tipo = u.Tipo_usuario.Valor
        }).ToList();

        return Ok(result);
    }

    // POST: api/usuarios
    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateUsuarioRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Usuario usuario = _mapper.Map<Usuario>(request);

        _context.Usuario.Add(usuario);
        _context.SaveChanges();

        return Created("", new { message = "usuario criado com sucesso" });
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var usuario = _context.Usuario.Find(id);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        _context.Usuario.Remove(usuario);
        _context.SaveChanges();

        return Ok("Usuário excluído com sucesso.");
    }
}