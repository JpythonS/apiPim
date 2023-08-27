using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Empresa {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}
    public string Nome {get; set;} = string.Empty;
    public string CpfCnpj {get; set;} = string.Empty;

    // public ICollection<Funcionario> funcionarios {get; set;} = null!;
}