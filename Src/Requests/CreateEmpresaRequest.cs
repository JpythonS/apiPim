namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateEmpresaRequest {
    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? CpfCnpj { get; set; }
}