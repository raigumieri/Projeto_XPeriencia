namespace XPeriencia.API.Models
{
    /// <summary>
    /// Representa uma reflexão pessoal registrada pelo usuário.
    /// Espaço para o usuário expressar sentimentos e pensamentos sobre sua jornada.
    /// </summary>
    public class Reflexao
    {
        /// <summary>
        /// Identificador único da reflexão (chave primária).
        /// Gerado automaticamente pelo banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Texto da reflexão ou descrição do sentimento.
        /// Pode conter pensamentos, emoções ou observações pessoais.
        /// Exemplo: "Hoje me senti motivado a evitar apostas impulsivas"
        /// </summary>
        public string Sentimento { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que a reflexão foi registrada.
        /// Permite acompanhamento cronológico da jornada emocional do usuário.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// ID do usuário que escreveu esta reflexão (chave estrangeira).
        /// Vincula a reflexão ao seu autor.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Objeto de navegação para o usuário autor da reflexão.
        /// Permite acesso aos dados do usuário através da reflexão.
        /// </summary>
        public Usuario Usuario { get; set; } = null!;
    }
}