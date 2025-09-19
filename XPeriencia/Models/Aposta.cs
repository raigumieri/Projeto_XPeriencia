using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPeriencia.Models
{
    public class Aposta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Descricao { get; set; }
        public int Valor { get; set; }
        public bool Ganhou { get; set; }
        public DateTime Data { get; set; }

    }
}
