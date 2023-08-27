using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Adicional
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public double ValorFixo { get; set; }
    public int? Porcentagem { get; set; }
    public int? MinSalario { get; set; }
    public int? MaxSalario { get; set; }

    [ForeignKey("TipoAdicional")]
    public int TipoAdicionalCod { get; set; }
    public TipoAdicional TipoAdicional { get; set; } = null!;

    // public ICollection<Pagamento>? Pagamentos { get; set; } 
    // public List<Funcionario> Funcionario { get; } = new();
    public ICollection<AdicionalFuncionario> AdicionalFuncionario { get; set;} = null!;

}