using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Pagamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime DataPagamento { get; set; }
    public double HorasTrabalhadas { get; set; }
    public double Valor { get; set; }

    [ForeignKey("Funcionario")]
    public int FuncionarioId { get; set; }

    public Funcionario Funcionario { get; set; } = null!;

    public ICollection<AdicionalPagamento> AdicionalPagamento { get; set;} = null!;
    public ICollection<DescontoPagamento> DescontoPagamento { get; set;} = null!;
}