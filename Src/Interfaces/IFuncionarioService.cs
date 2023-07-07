namespace api_pim.Interfaces;

using api_pim.Entities;

public interface IFuncionarioService{

    public List<FuncionarioDto> GetFuncionarios(string filtro);
}