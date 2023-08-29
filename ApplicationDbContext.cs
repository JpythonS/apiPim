using Microsoft.EntityFrameworkCore;

using api_pim.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<TipoUsuario> TipoUsuario { get; set; } = null!;
    public DbSet<TipoCargo> TipoCargo { get; set; } = null!;
    public DbSet<TipoDesconto> TipoDesconto { get; set; } = null!;
    public DbSet<TipoAdicional> TipoAdicional { get; set; } = null!;
    public DbSet<TipoPagamento> TipoPagamento { get; set; } = null!;

    public DbSet<Empresa> Empresas { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Desconto> Descontos { get; set; } = null!;
    public DbSet<Adicional> Adicionais { get; set; } = null!;
    public DbSet<Pagamento> Pagamento { get; set; } = null!;
    public DbSet<Funcionario> Funcionarios { get; set; } = null!;

    public DbSet<AdicionalPagamento> AdicionalPagamento { get; set; } = null!;
    public DbSet<AdicionalFuncionario> AdicionalFuncionario { get; set; } = null!;

    public DbSet<DescontoPagamento> DescontoPagamento { get; set; } = null!;
    public DbSet<DescontoFuncionario> DescontoFuncionario { get; set; } = null!;
}
