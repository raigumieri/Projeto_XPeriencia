using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;
using XPeriencia.API.Models;

namespace XPeriencia.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de reflexões pessoais.
    /// Permite que usuários registrem pensamentos e sentimentos sobre sua jornada.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReflexoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReflexoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todas as reflexões do sistema.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reflexao>>> GetReflexoes()
        {
            return await _context.Reflexoes
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        /// <summary>
        /// Busca uma reflexão específica por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Reflexao>> GetReflexao(int id)
        {
            var reflexao = await _context.Reflexoes
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reflexao == null)
            {
                return NotFound();
            }

            return reflexao;
        }

        /// <summary>
        /// Retorna todas as reflexões de um usuário específico.
        /// Permite acompanhamento da jornada emocional do usuário.
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Reflexao>>> GetReflexoesPorUsuario(int usuarioId)
        {
            var reflexoes = await _context.Reflexoes
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Usuario)
                .ToListAsync();

            return reflexoes;
        }

        /// <summary>
        /// Busca reflexões que contenham determinado sentimento.
        /// Busca parcial (Contains) e case-insensitive para maior flexibilidade.
        /// </summary>
        [HttpGet("sentimento/{sentimento}")]
        public async Task<ActionResult<IEnumerable<Reflexao>>> GetReflexoesPorSentimento(string sentimento)
        {
            var reflexoes = await _context.Reflexoes
                .Where(r => r.Sentimento.ToLower().Contains(sentimento.ToLower()))
                .Include(r => r.Usuario)
                .ToListAsync();

            return reflexoes;
        }

        /// <summary>
        /// Registra uma nova reflexão no sistema.
        /// Valida se o usuário existe antes de criar.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Reflexao>> PostReflexao(Reflexao reflexao)
        {
            // Verificar se o usu�rio existe
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == reflexao.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest("Usu�rio n�o encontrado.");
            }

            reflexao.Data = DateTime.Now;
            _context.Reflexoes.Add(reflexao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReflexao), new { id = reflexao.Id }, reflexao);
        }

        /// <summary>
        /// Atualiza uma reflexão existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReflexao(int id, Reflexao reflexao)
        {
            if (id != reflexao.Id)
            {
                return BadRequest();
            }

            _context.Entry(reflexao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReflexaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma reflexão do sistema.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReflexao(int id)
        {
            var reflexao = await _context.Reflexoes.FindAsync(id);
            if (reflexao == null)
            {
                return NotFound();
            }

            _context.Reflexoes.Remove(reflexao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReflexaoExists(int id)
        {
            return _context.Reflexoes.Any(e => e.Id == id);
        }
    }
}

