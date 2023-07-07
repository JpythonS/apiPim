using Microsoft.EntityFrameworkCore;

using api_pim.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<TipoUsuario> TipoUsuario { get; set; } = null!;
    public DbSet<TipoCargo> TipoCargo { get; set; } = null!;

    public DbSet<Empresa> Empresa { get; set; } = null!;

    public DbSet<Usuario> Usuario { get; set; } = null!;
    public DbSet<Funcionario> Funcionario { get; set; } = null!;
    
}
