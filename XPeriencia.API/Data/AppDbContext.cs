using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Models;

namespace XPeriencia.API.Data
{
    /// <summary>
    /// Contexto do banco de dados da aplicação XPeriência.
    /// Responsável por gerenciar as entidades e suas configurações no Entity Framework Core.
    /// Herda de DbContext para fornecer funcionalidades de ORM.
    /// </summary>
    public class AppDbContext : DbContext
    {

        /// <summary>
        /// Construtor que recebe as opções de configuração do contexto.
        /// Injetado via Dependency Injection no ASP.NET Core.
        /// </summary>
        /// <param name="options">Opções de configuração incluindo connection string e provedor de banco</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets - Representam as tabelas no banco de dados

        /// <summary>
        /// Tabela de Usuários no banco de dados.
        /// Permite realizar operações CRUD através do Entity Framework.
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Tabela de Apostas no banco de dados.
        /// Armazena todas as apostas registradas pelos usuários.
        /// </summary>
        public DbSet<Aposta> Apostas { get; set; }

        /// <summary>
        /// Tabela de Reflexões no banco de dados.
        /// Contém as reflexões pessoais escritas pelos usuários.
        /// </summary>
        public DbSet<Reflexao> Reflexoes { get; set; }

        /// <summary>
        /// Configura o modelo de dados durante a criação do banco.
        /// Define relacionamentos, índices, restrições e configurações específicas.
        /// Chamado automaticamente pelo Entity Framework durante as migrations.
        /// </summary>
        /// <param name="modelBuilder">Construtor do modelo para configurações fluent API</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de índice único para email de usuário
            // Garante que não existam dois usuários com o mesmo email
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuração de precisão decimal para valores monetários
            // Define 18 dígitos totais com 2 casas decimais
            modelBuilder.Entity<Aposta>()
                .Property(a => a.Valor)
                .HasPrecision(18, 2);

            // Configuração do relacionamento Usuario -> Apostas
            // Um usuário possui muitas apostas (1:N)
            // DeleteBehavior.Cascade: ao excluir um usuário, suas apostas são excluídas automaticamente
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Apostas)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento Usuario -> Reflexões
            // Um usuário possui muitas reflexões (1:N)
            // DeleteBehavior.Cascade: ao excluir um usuário, suas reflexões são excluídas automaticamente
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Reflexoes)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}