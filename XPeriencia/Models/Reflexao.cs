using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPeriencia.Models
{
    /// <summary>
    /// Representa uma reflexão ou sentimento registrado pelo usuário.
    /// </summary>
    public class Reflexao
    {
        /// <summary>
        /// Identificador único da reflexão.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do usuário que fez a reflexão.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Data e hora em que a reflexão foi registrada. Inicializa com a data e hora atual.
        /// </summary>
        public DateTime Data { get; set; } = DateTime.Now;

        /// <summary>
        /// Descrição do sentimento ou reflexão do usuário.
        /// </summary>
        public string Sentimento { get; set; }
    }
}
