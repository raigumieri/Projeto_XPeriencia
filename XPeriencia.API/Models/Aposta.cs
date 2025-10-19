namespace XPeriencia.API.Models
{
    /// <summary>
    /// Representa uma aposta fictícia registrada por um usuário.
    /// Utilizada para fins de monitoramento e reflexão sobre hábitos de apostas.
    /// </summary>
    public class Aposta
    {
        /// <summary>
        /// Identificador único da aposta (chave primária).
        /// Gerado automaticamente pelo banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição detalhada da aposta realizada.
        /// Exemplo: "Aposta no jogo do Palmeiras vs Flamengo"
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor monetário apostado.
        /// Configurado com 18 dígitos e 2 casas decimais no banco.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Resultado da aposta.
        /// Valores comuns: "Vitória", "Derrota", "Empate"
        /// Usado para cálculos estatísticos e relatórios.
        /// </summary>
        public string Resultado { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que a aposta foi registrada.
        /// Usado para filtros por período e ordenação cronológica.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// ID do usuário que registrou esta aposta (chave estrangeira).
        /// Relaciona a aposta ao seu proprietário.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Objeto de navegação para o usuário proprietário da aposta.
        /// Permite acesso aos dados do usuário através da aposta.
        /// </summary>
        public Usuario Usuario { get; set; } = null!;

    }
}