using Microsoft.EntityFrameworkCore;
using Noryx.API.Models;

namespace Noryx.API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Conta> Contas { get; set; } = null!;
        public DbSet<Transacao> Transacoes { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<InstituicaoFinanceira> InstituicoesFinanceiras { get; set; } = null!;
        public DbSet<Pais> Paises { get; set; } = null!;
        public DbSet<Moeda> Moedas { get; set; } = null!;
        public DbSet<Cotacao> Cotacoes { get; set; } = null!;
        public DbSet<HistoricoCotacao> HistoricosCotacoes { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.SenhaHash)
                    .IsRequired();
            });


            modelBuilder.Entity<InstituicaoFinanceira>(entity =>
            {
                entity.ToTable("InstituicaoFinanceira");

                entity.Property(i => i.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Conta>(entity =>
            {
                entity.ToTable("Conta");

                entity.Property(c => c.Descricao)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Saldo)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(c => c.Usuario)
                    .WithMany(u => u.Contas)
                    .HasForeignKey(c => c.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.InstituicaoFinanceira)
                    .WithMany(i => i.Contas)
                    .HasForeignKey(c => c.InstituicaoFinanceiraId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categoria");

                entity.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(c => c.Nome)
                    .IsUnique();
            });


            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.ToTable("Transacao");

                entity.Property(t => t.Valor)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(t => t.Conta)
                    .WithMany(c => c.Transacoes)
                    .HasForeignKey(t => t.ContaId);

                entity.HasOne(t => t.Categoria)
                    .WithMany(c => c.Transacoes)
                    .HasForeignKey(t => t.CategoriaId);
            });


            modelBuilder.Entity<Pais>(entity =>
            {
                entity.ToTable("Pais");

                entity.Property(p => p.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Moeda>(entity =>
            {
                entity.ToTable("Moeda");

                entity.Property(m => m.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Codigo)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.HasIndex(m => m.Codigo)
                    .IsUnique();
            });


            modelBuilder.Entity<Pais>()
            .HasMany(p => p.Moedas)
            .WithMany(m => m.Paises)
            .UsingEntity(j => j.ToTable("PaisMoeda"));


            modelBuilder.Entity<Cotacao>(entity =>
            {
                entity.ToTable("Cotacao");

                entity.Property(c => c.ValorCompra).HasColumnType("decimal(18,6)");
                entity.Property(c => c.ValorVenda).HasColumnType("decimal(18,6)");
                entity.Property(c => c.Variacao).HasColumnType("decimal(18,6)");

                entity.HasOne(c => c.Moeda)
                    .WithMany(m => m.Cotacoes)
                    .HasForeignKey(c => c.MoedaId);
            });


            modelBuilder.Entity<HistoricoCotacao>(entity =>
            {
                entity.ToTable("HistoricoCotacao");

                entity.Property(h => h.Valor)
                    .HasColumnType("decimal(18,6)");

                entity.Property(h => h.MoedaOrigem)
                    .IsRequired()
                    .HasMaxLength(3); // código ISO da moeda, ex: USD

                entity.Property(h => h.MoedaDestino)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(h => h.DataHora)
                    .IsRequired();
            });

            modelBuilder.Entity<Pais>().HasData(
                new Pais { Id = 1, Nome = "Brasil" },
                new Pais { Id = 2, Nome = "Estados Unidos" }
            );

            modelBuilder.Entity<Moeda>().HasData(
                new Moeda { Id = 1, Nome = "Real", Codigo = "BRL" },
                new Moeda { Id = 2, Nome = "Dólar Americano", Codigo = "USD" }
            );

            modelBuilder.Entity<Pais>()
            .HasMany(p => p.Moedas)
            .WithMany(m => m.Paises)
            .UsingEntity(j => j.HasData(
                new { PaisesId = 1, MoedasId = 1 }, // Brasil -> BRL
                new { PaisesId = 2, MoedasId = 2 } // EUA -> USD
            ));

        }

    }
}
