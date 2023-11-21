namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateFuncionarioRequest
{
    [Required]
    public string? NomeCompleto { get; set; }

    public DateTime? DataNascimento { get; set; }

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
    public double? JornadaTrabalhoSemanal { get; set; }

    [Required]
    [EmailAddress]
    public string? UsuarioEmail { get; set; }

    [Required]
    public int? EmpresaId { get; set; }

    [Required]
    public string? Rg { get; set; }

    [Required]
    public string? Celular { get; set; }

    [Required]
    public string? CelularContatoEmergencia { get; set; }

    [Required]
    public string? Bairro { get; set; }

    [Required]
    public string? Cidade { get; set; }

    [Required]
    public string? Estado { get; set; }

    [Required]
    public string? Pis { get; set; }

    // required neles?
    public string? AgenciaBancaria { get; set; }
    public string? DigitoAgencia { get; set; }
    public string? ContaBancaria { get; set; }
    public string? DigitoConta { get; set; }
    public string? Banco { get; set; }
}