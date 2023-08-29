using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class AdicionalPagamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [ForeignKey("Pagamento")]
    public int PagamentoId { get; set; }
    public Pagamento Pagamento { get; set; } = null!;

    [ForeignKey("Adicional")]
    public int AdicionalId { get; set; }
    public Adicional Adicional { get; set; } = null!;
}