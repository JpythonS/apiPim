namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateFuncionarioRequest {
    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Endereco { get; set; }

    [Required]
    [StringLength(11)]
    public string? Cpf { get; set; }

    [Required]
    public int? TipoCargoCod { get; set; }

    [Required]
    public double? SalarioBase { get; set; }

    [Required]
    public double? JornadaTrabalhoSemanal {get; set;}

    [Required]
    [EmailAddress]
    public string? UsuarioEmail { get; set; }

    [Required]
    public int? EmpresaId { get; set; }
}