namespace XPeriencia.API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public int Pontos { get; set; }

        // Relacionamentos
        public ICollection<Aposta> Apostas { get; set; } = new List<Aposta>();
        public ICollection<Reflexao> Reflexoes { get; set; } = new List<Reflexao>();
    }
}