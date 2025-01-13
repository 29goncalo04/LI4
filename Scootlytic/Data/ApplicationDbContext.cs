using Microsoft.EntityFrameworkCore;
using Scootlytic.Models;

namespace Scootlytic.Data  // Certifica-te de que o namespace está correto
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Definir as entidades (tabelas) que vão ser mapeadas
        public DbSet<User> Users { get; set; }
        // public DbSet<Product> Products { get; set; }

        // Outras entidades podem ser adicionadas aqui
    }
}
