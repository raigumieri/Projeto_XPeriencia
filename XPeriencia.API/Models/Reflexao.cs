namespace XPeriencia.API.Models
{
    public class Reflexao
    {
        public int Id { get; set; }
        public string Sentimento { get; set; } = string.Empty;
        public DateTime Data { get; set; }

        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}