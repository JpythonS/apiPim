namespace api_pim.Services;
using api_pim.Interfaces;
using api_pim.Models;
using api_pim.Entities;

public class FuncionarioService : IFuncionarioService
{
    private readonly ApplicationDbContext _context;
    public FuncionarioService(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<FuncionarioDto> GetFuncionarios(string filtro = "")
    {
        var funcionarios = _context.Funcionario.AsQueryable();

        // implementar a opcao de concatenar filtros ou sempre fazer todos?
        // ignorar acentos?

        if (!string.IsNullOrEmpty(filtro))
        {
            string filtroLowerCase = filtro.ToLower();

            funcionarios = funcionarios
             .Where(funcionario =>
                funcionario.Nome.ToLower().Contains(filtroLowerCase) ||
                funcionario.Sobrenome.ToLower().Contains(filtroLowerCase) || //juntar com nome
                funcionario.Cpf.ToLower().Contains(filtroLowerCase) ||
                funcionario.Salario_base.ToString().ToLower().Contains(filtroLowerCase) ||
                funcionario.Jornada_trabalho_semanal.ToString().ToLower().Contains(filtroLowerCase) ||
                funcionario.TipoCargo.Valor.ToLower().Contains(filtroLowerCase) ||
                funcionario.Usuario.Email.ToLower().Contains(filtroLowerCase)
             );
        }

        var result = funcionarios.Select(funcionario => new FuncionarioDto
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Sobrenome = funcionario.Sobrenome,
            Cpf = funcionario.Cpf,
            Cargo = funcionario.TipoCargo.Valor,
            SalarioBase = funcionario.Salario_base,
            JornadaTrabalhoSemanal = funcionario.Jornada_trabalho_semanal,
            Email = funcionario.Usuario.Email
        }).ToList();

        return result;
    }


    public void CreateFuncionario(Funcionario funcionario) {
        _context.Funcionario.Add(funcionario);
        _context.SaveChanges();
    } 
}