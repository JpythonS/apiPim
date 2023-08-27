using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class AdicionalFuncionario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [ForeignKey("Funcionario")]
    public int FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; } = null!;

    [ForeignKey("Adicional")]
    public int AdicionalId { get; set; }
    public Adicional Adicional { get; set; } = null!;
}