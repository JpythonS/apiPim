namespace api_pim.Interfaces;

using api_pim.Models;

public interface IUsuarioService
{
    List<UsuarioDto> GetUsuarios(string filtro);
}