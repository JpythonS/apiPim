using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TipoAdicional {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Cod {get; set;}
    public string Valor {get; set;} = string.Empty;
}