using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Models;

namespace XPeriencia.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Aposta> Apostas { get; set; }
        public DbSet<Reflexao> Reflexoes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura��es adicionais
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Aposta>()
                .Property(a => a.Valor)
                .HasPrecision(18, 2);

            // Configura��o de relacionamentos
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Apostas)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Reflexoes)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}