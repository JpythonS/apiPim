using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class DescontoPagamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [ForeignKey("Pagamento")]
    public int PagamentoId { get; set; }
    public Pagamento Pagamento { get; set; } = null!;

    [ForeignKey("Desconto")]
    public int DescontoId { get; set; }
    public Desconto Desconto { get; set; } = null!;
}