using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;
using XPeriencia.API.Models;

namespace XPeriencia.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de apostas fictícias.
    /// Permite registro, consulta e análise de apostas por usuário.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApostasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApostasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todas as apostas do sistema.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostas()
        {
            return await _context.Apostas
                .Include(a => a.Usuario)
                .ToListAsync();
        }

        /// <summary>
        /// Busca uma aposta específica por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Aposta>> GetAposta(int id)
        {
            var aposta = await _context.Apostas
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aposta == null)
            {
                return NotFound();
            }

            return aposta;
        }

        /// <summary>
        /// Retorna todas as apostas de um usuário específico.
        /// Útil para exibir histórico pessoal.
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasPorUsuario(int usuarioId)
        {
            var apostas = await _context.Apostas
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.Usuario)
                .ToListAsync();

            return apostas;
        }

        /// <summary>
        /// Filtra apostas por resultado (Vitória, Derrota, Empate).
        /// Case-insensitive para facilitar busca.
        /// </summary>
        [HttpGet("resultado/{resultado}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasPorResultado(string resultado)
        {
            var apostas = await _context.Apostas
                .Where(a => a.Resultado.ToLower() == resultado.ToLower())
                .Include(a => a.Usuario)
                .ToListAsync();

            return apostas;
        }

        /// <summary>
        /// Registra uma nova aposta no sistema.
        /// Valida se o usuário existe antes de criar.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Aposta>> PostAposta(Aposta aposta)
        {
            
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == aposta.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest("Usu�rio n�o encontrado.");
            }

            aposta.Data = DateTime.Now;
            _context.Apostas.Add(aposta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAposta), new { id = aposta.Id }, aposta);
        }

        /// <summary>
        /// Atualiza uma aposta existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAposta(int id, Aposta aposta)
        {
            if (id != aposta.Id)
            {
                return BadRequest();
            }

            _context.Entry(aposta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApostaExists(id))
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
        /// Remove uma aposta do sistema.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAposta(int id)
        {
            var aposta = await _context.Apostas.FindAsync(id);
            if (aposta == null)
            {
                return NotFound();
            }

            _context.Apostas.Remove(aposta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApostaExists(int id)
        {
            return _context.Apostas.Any(e => e.Id == id);
        }
    }
}