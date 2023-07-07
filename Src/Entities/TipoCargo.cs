using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class TipoCargo {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Cod {get; set;}
    public string Valor {get; set;} = string.Empty;
}