using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace XPeriencia.API.Data
{
    /// <summary>
    /// Factory para criação do DbContext em tempo de design.
    /// Necessária para que as ferramentas do Entity Framework (migrations, scaffolding)
    /// possam criar instâncias do contexto sem executar a aplicação completa.
    /// Implementa IDesignTimeDbContextFactory para suporte às ferramentas CLI do EF Core.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Cria uma instância do AppDbContext configurada para tempo de design.
        /// Usado pelos comandos: dotnet ef migrations add, dotnet ef database update, etc.
        /// </summary>
        /// <param name="args">Argumentos da linha de comando (não utilizados neste caso)</param>
        /// <returns>Instância configurada do AppDbContext com SQLite</returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            // Cria o builder de opções para o DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Configura para usar SQLite com o arquivo de banco especificado
            // "Data Source=xperiencia.db" cria/usa o arquivo xperiencia.db na raiz do projeto
            optionsBuilder.UseSqlite("Data Source=xperiencia.db");

            // Retorna a instância do contexto com as configurações aplicadas
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}