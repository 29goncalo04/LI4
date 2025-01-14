using Microsoft.EntityFrameworkCore;
using Scootlytic.Models;

namespace Scootlytic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Definir as entidades (tabelas) que vão ser mapeadas
        public DbSet<User> Users { get; set; }
        public DbSet<Encomenda> Encomendas { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<Trotinete> Trotinetes { get; set; }
        public DbSet<Escolhe> Escolhe { get; set; }
        public DbSet<Adicionada> Adicionada { get; set; }
        public DbSet<Peca> Pecas { get; set; }
        public DbSet<Passo> Passos { get; set; }
        public DbSet<Possui> Possui { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento entre Encomenda e User
            modelBuilder.Entity<Encomenda>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.EmailUtilizador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Carrinho) // O User tem um Carrinho
                .WithOne(c => c.User) // O Carrinho tem um único User
                .HasForeignKey<User>(u => u.CartId) // A chave estrangeira está no campo CartId do User
                .OnDelete(DeleteBehavior.Restrict); // Não deletar o Carrinho quando o User for deletado



            // Relacionamento entre Trotinete e Encomenda
            modelBuilder.Entity<Trotinete>()
                .HasOne(t => t.Encomenda)
                .WithMany()
                .HasForeignKey(t => t.NumeroEncomenda)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Escolhe, User e Trotinete
            modelBuilder.Entity<Escolhe>()
                .HasKey(e => new { e.EmailUtilizador, e.IdTrotinete });

            modelBuilder.Entity<Escolhe>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.EmailUtilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Escolhe>()
                .HasOne(e => e.Trotinete)
                .WithMany()
                .HasForeignKey(e => e.IdTrotinete)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Adicionada, Carrinho e Trotinete
            modelBuilder.Entity<Adicionada>()
                .HasKey(a => new { a.IdCarrinho, a.IdTrotinete });

            modelBuilder.Entity<Adicionada>()
                .HasOne(a => a.Carrinho)
                .WithMany()
                .HasForeignKey(a => a.IdCarrinho)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Adicionada>()
                .HasOne(a => a.Trotinete)
                .WithMany()
                .HasForeignKey(a => a.IdTrotinete)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Passo e Peça
            modelBuilder.Entity<Passo>()
                .HasOne(p => p.Peca)
                .WithMany()
                .HasForeignKey(p => p.ReferenciaPeca)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Possui, Trotinete e Passo
            modelBuilder.Entity<Possui>()
                .HasKey(p => new { p.IdTrotinete, p.IdPasso }); // Chave composta

            modelBuilder.Entity<Possui>()
                .HasOne(p => p.Trotinete)
                .WithMany()
                .HasForeignKey(p => p.IdTrotinete)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Possui>()
                .HasOne(p => p.Passo)
                .WithMany()
                .HasForeignKey(p => p.IdPasso)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}