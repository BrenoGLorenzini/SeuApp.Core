using Microsoft.EntityFrameworkCore;
using SeuApp.Core.Entities;

namespace SeuApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<VendaItem> VendaItens => Set<VendaItem>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Produto>(e =>
        {
            e.Property(p => p.Descricao).HasMaxLength(150).IsRequired();
            e.Property(p => p.Valor).HasColumnType("decimal(10,2)").IsRequired();
        });

        mb.Entity<Venda>(e =>
        {
            e.Property(v => v.NomeCliente).HasMaxLength(120).IsRequired();
            e.Property(v => v.Cpf).HasMaxLength(14);
            e.Property(v => v.Total).HasColumnType("decimal(10,2)").HasDefaultValue(0);
        });

        mb.Entity<VendaItem>(e =>
        {
            e.Property(i => i.Descricao).HasMaxLength(150).IsRequired();
            e.Property(i => i.Quantidade).HasColumnType("decimal(10,2)").IsRequired();
            e.Property(i => i.ValorUnit).HasColumnType("decimal(10,2)").IsRequired();
        });

        mb.Entity<Fornecedor>(e =>
        {
            e.Property(f => f.Nome).HasMaxLength(120).IsRequired();
        });

        base.OnModelCreating(mb);
    }
}
