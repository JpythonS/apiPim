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

    public UsuarioController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Rota GET: api/usuario
    // params: filtro [string][optional] (filtra usuario por email)
    [HttpGet]
    [Authorize]
    public IActionResult Get(string filtro = "")
    {
        try
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
        catch (Exception)
        {
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.GU}]");
        }
    }

    // POST: api/usuario
    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateUsuarioRequest request)
    {
        try
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
        catch (Exception)
        {
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
            var usuario = _context.Usuario.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _context.Usuario.Remove(usuario);
            _context.SaveChanges();

            return Ok("Usuário excluído com sucesso.");
        }
        catch (Exception)
        {
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.DU}]");
        }
    }
}
