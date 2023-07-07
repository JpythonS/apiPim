namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class LoginRequest {
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    //adicionar regras de texto da senha -> documentar essas regras
    public string? Senha { get; set; }
}