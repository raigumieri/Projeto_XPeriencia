using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPeriencia.Models
{
    /// <summary>
    /// Representa um usuário do sistema.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do usuário.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Endereço de email do usuário.
        /// </summary>
        public string Email {get ; set; }

        /// <summary>
        /// Data de criação do usuário. 
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Quantidade de pontos acumulados pelo usuário. Inicializa com 0.
        /// </summary>
        public int Pontos { get; set; } = 0;

    }
}
