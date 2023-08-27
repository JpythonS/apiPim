namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateUsuarioRequest {
    [Required]
    public int? TipoUsuarioCod { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$", ErrorMessage = "A senha deve conter pelo menos 1 caractere maiúsculo, 1 minúsculo, 1 caractere especial, 1 número e ter no mínimo 8 caracteres.")]
    public string? Senha { get; set; }
}