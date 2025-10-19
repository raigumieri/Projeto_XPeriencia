using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;
using XPeriencia.API.Models;

namespace XPeriencia.API.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de usuários.
    /// Implementa operações CRUD completas e consultas personalizadas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase 
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados com suas apostas e reflexões.
        /// Include() carrega os relacionamentos para evitar lazy loading.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Include(u => u.Apostas)
                .Include(u => u.Reflexoes)
                .ToListAsync();
        }

        /// <summary>
        /// Busca um usuário específico por ID.
        /// Retorna 404 se não encontrado.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Apostas)
                .Include(u => u.Reflexoes)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// Busca usuário por email (único no sistema).
        /// Útil para login ou verificação de duplicidade.
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByEmail(string email)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Apostas)
                .Include(u => u.Reflexoes)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// Cria um novo usuário no sistema.
        /// DataCriacao é preenchida automaticamente com a data atual.
        /// Retorna 201 Created com a localização do recurso criado.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.DataCriacao = DateTime.Now;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// Valida se o ID da URL corresponde ao ID do objeto.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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
        /// Remove um usuário do sistema.
        /// Apostas e reflexões relacionadas são deletadas automaticamente (Cascade).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Método auxiliar para verificar se um usuário existe.
        /// Usado internamente para validações.
        /// </summary>
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}