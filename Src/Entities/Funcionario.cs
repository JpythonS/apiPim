using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Funcionario {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 

    public string Nome { get; set; } = string.Empty;

    public string Sobrenome { get; set; } = string.Empty; 

    public string Cpf { get; set; } = string.Empty;

    [ForeignKey("TipoCargo")]
    public int Tipo_cargo_cod { get; set; }

    public TipoCargo TipoCargo { get; set; } = null!;

    public double Salario_base { get; set; }

    public double Jornada_trabalho_semanal {get; set;}

    [ForeignKey("Usuario")]
    public string Usuario_id { get; set; } = string.Empty;

    public Usuario Usuario { get; set; } = null!;

    [ForeignKey("Empresa")]
    public int Empresa_id { get; set; }

    public Empresa Empresa { get; set; } = null!;
}