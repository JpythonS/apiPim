namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateFuncionarioRequest {
    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Sobrenome { get; set; }

    [Required]
    [StringLength(11)]
    public string? Cpf { get; set; }

    [Required]
    public int? Tipo_cargo_cod { get; set; }

    [Required]
    public double? Salario_base { get; set; }

    [Required]
    public double? Jornada_trabalho_semanal {get; set;}

    [Required]
    [EmailAddress]
    public string? Usuario_email { get; set; }

    [Required]
    public int? Empresa_id { get; set; }
}