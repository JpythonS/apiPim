using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_pim.Entities;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Tipo_usuario")]
    public int Tipo_usuario_cod { get; set; }
    public TipoUsuario Tipo_usuario { get; set; } = null!;
    
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

