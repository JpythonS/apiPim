using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using api_pim.Entities;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoUsuarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TipoUsuarioController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TipoUsuario>>> Get()
    {
        return await _context.TipoUsuario.ToListAsync();
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] TipoUsuario request)
    {
        if (request == null) {
            return BadRequest(new { message = "Dados do tipo de usuário inválidos." });
        }

        _context.TipoUsuario.Add(request);
        _context.SaveChanges();

        return Ok($"Tipo de usuario [{request.Valor}] criado com sucesso");
    }
}