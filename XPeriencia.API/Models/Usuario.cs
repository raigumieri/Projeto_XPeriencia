namespace XPeriencia.API.Models
{
    /// <summary>
    /// Representa um usuário do sistema XPeriência.
    /// Cada usuário pode ter múltiplas apostas e reflexões associadas.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário (chave primária).
        /// Gerado automaticamente pelo banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// Campo obrigatório.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email único do usuário para identificação.
        /// Possui índice único no banco de dados para evitar duplicatas.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que o usuário foi criado no sistema.
        /// Registra automaticamente quando o usuário se cadastra.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Sistema de pontos do usuário.
        /// Pode ser usado para gamificação ou acompanhamento de progresso.
        /// </summary>
        public int Pontos { get; set; }

        /// <summary>
        /// Coleção de apostas registradas pelo usuário.
        /// Relacionamento um-para-muitos: um usuário pode ter várias apostas.
        /// </summary>
        public ICollection<Aposta> Apostas { get; set; } = new List<Aposta>();

        /// <summary>
        /// Coleção de reflexões escritas pelo usuário.
        /// Relacionamento um-para-muitos: um usuário pode ter várias reflexões.
        /// </summary>
        public ICollection<Reflexao> Reflexoes { get; set; } = new List<Reflexao>();
    }
}