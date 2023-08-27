using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Funcionario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string NomeCompleto { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    [ForeignKey("TipoCargo")]
    public int TipoCargoCod { get; set; }

    public TipoCargo TipoCargo { get; set; } = null!;

    public double SalarioBase { get; set; }

    public double JornadaTrabalhoSemanal { get; set; }

    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;

    [ForeignKey("Empresa")]
    public int EmpresaId { get; set; }

    public Empresa Empresa { get; set; } = null!;

    // public ICollection<Desconto> Descontos { get; set; } = new List<Desconto>();
    // public List<Adicional> Adicional { get; } = new();

    public ICollection<AdicionalFuncionario> AdicionalFuncionario { get; set;} = null!;
    public ICollection<DescontoFuncionario> DescontoFuncionario { get; set;} = null!;

}