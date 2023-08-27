using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class DescontoFuncionario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [ForeignKey("Funcionario")]
    public int FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; } = null!;

    [ForeignKey("Desconto")]
    public int DescontoId { get; set; }
    public Desconto Desconto { get; set; } = null!;
}