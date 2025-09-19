using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPeriencia.Models
{
    public class Reflexao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string Sentimento { get; set; }
    }
}
