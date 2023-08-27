using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Tipo_usuario")]
    public int TipoUsuarioCod { get; set; }
    public TipoUsuario TipoUsuario { get; set; } = null!;
    
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

