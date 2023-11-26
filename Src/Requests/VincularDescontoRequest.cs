namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class VincularDescontoRequest
{
    [Required]
    public int FuncionarioId { get; set; }

    [Required]

    public double ValorFixo { get; set; }

    [Required]
    public int TipoDescontoCod { get; set; }
}