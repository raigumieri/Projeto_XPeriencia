namespace XPeriencia.API.Models
{
    public class Aposta
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Resultado { get; set; } = string.Empty;
        public DateTime Data { get; set; }

        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

    }
}