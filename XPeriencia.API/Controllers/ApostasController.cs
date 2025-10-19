using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;
using XPeriencia.API.Models;

namespace XPeriencia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApostasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApostasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Apostas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostas()
        {
            return await _context.Apostas
                .Include(a => a.Usuario)
                .ToListAsync();
        }

        // GET: api/Apostas/5
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

        // GET: api/Apostas/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasPorUsuario(int usuarioId)
        {
            var apostas = await _context.Apostas
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.Usuario)
                .ToListAsync();

            return apostas;
        }

        // GET: api/Apostas/resultado/Vitoria
        [HttpGet("resultado/{resultado}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasPorResultado(string resultado)
        {
            var apostas = await _context.Apostas
                .Where(a => a.Resultado.ToLower() == resultado.ToLower())
                .Include(a => a.Usuario)
                .ToListAsync();

            return apostas;
        }

        // POST: api/Apostas
        [HttpPost]
        public async Task<ActionResult<Aposta>> PostAposta(Aposta aposta)
        {
            // Verificar se o usuário existe
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == aposta.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest("Usuário não encontrado.");
            }

            aposta.Data = DateTime.Now;
            _context.Apostas.Add(aposta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAposta), new { id = aposta.Id }, aposta);
        }

        // PUT: api/Apostas/5
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

        // DELETE: api/Apostas/5
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