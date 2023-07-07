namespace api_pim.Services;


using api_pim.Models;
using api_pim.Interfaces;

public class UsuarioService : IUsuarioService {
    private readonly ApplicationDbContext _context;

    public UsuarioService(ApplicationDbContext context) {
        _context = context;
    }

    public List<UsuarioDto> GetUsuarios(string filtro) {
        var usuarios = _context.Usuario.AsQueryable();

        if (!string.IsNullOrEmpty(filtro)) {
            usuarios = usuarios
             .Where(u => u.Email.ToLower().Contains(filtro.ToLower()));
        }

        return usuarios.Select(u => new UsuarioDto {
            Email = u.Email,
            Tipo = u.Tipo_usuario.Valor
        }).ToList();
    }
}