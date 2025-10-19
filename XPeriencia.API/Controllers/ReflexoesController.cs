using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;
using XPeriencia.API.Models;

namespace XPeriencia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReflexoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReflexoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reflexoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reflexao>>> GetReflexoes()
        {
            return await _context.Reflexoes
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        // GET: api/Reflexoes/5
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

        // GET: api/Reflexoes/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Reflexao>>> GetReflexoesPorUsuario(int usuarioId)
        {
            var reflexoes = await _context.Reflexoes
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Usuario)
                .ToListAsync();

            return reflexoes;
        }

        // GET: api/Reflexoes/sentimento/Feliz
        [HttpGet("sentimento/{sentimento}")]
        public async Task<ActionResult<IEnumerable<Reflexao>>> GetReflexoesPorSentimento(string sentimento)
        {
            var reflexoes = await _context.Reflexoes
                .Where(r => r.Sentimento.ToLower().Contains(sentimento.ToLower()))
                .Include(r => r.Usuario)
                .ToListAsync();

            return reflexoes;
        }

        // POST: api/Reflexoes
        [HttpPost]
        public async Task<ActionResult<Reflexao>> PostReflexao(Reflexao reflexao)
        {
            // Verificar se o usuário existe
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == reflexao.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest("Usuário não encontrado.");
            }

            reflexao.Data = DateTime.Now;
            _context.Reflexoes.Add(reflexao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReflexao), new { id = reflexao.Id }, reflexao);
        }

        // PUT: api/Reflexoes/5
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

        // DELETE: api/Reflexoes/5
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

