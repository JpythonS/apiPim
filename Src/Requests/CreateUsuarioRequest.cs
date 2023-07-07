namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateUsuarioRequest {
    [Required]
    public int? Tipo_usuario_cod { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Senha { get; set; }
}