using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Desconto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public double ValorFixo { get; set; }
    public int? Porcentagem { get; set; }
    public int? MinSalario { get; set; }
    public int? MaxSalario { get; set; }

    [ForeignKey("TipoDesconto")]
    public int TipoDescontoCod { get; set; }
    public TipoDesconto TipoDesconto { get; set; } = null!;
}