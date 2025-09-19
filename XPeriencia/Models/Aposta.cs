using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPeriencia.Models

/// <summary>
/// Representa uma aposta feita por um usuário.
/// </summary>
{
    public class Aposta
    {
        /// <summary>
        /// Identificador único da aposta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do usuário que fez a aposta.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Descrição da aposta (ex: qual jogo, qual evento).
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Valor da aposta em pontos.
        /// </summary>
        public int Valor { get; set; }

        /// <summary>
        /// Se o usuário ganhou (true) ou perdeu (false) a aposta.
        /// </summary>
        public bool Ganhou { get; set; }

        /// <summary>
        /// Data e hora em que a aposta foi feita.
        /// </summary>
        public DateTime Data { get; set; }

    }
}
