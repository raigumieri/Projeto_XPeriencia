using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;

namespace XPeriencia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Relatorios/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<object>> GetRelatorioUsuario(int usuarioId)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Apostas)
                .Include(u => u.Reflexoes)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // LINQ: Estatísticas de apostas
            var totalApostas = usuario.Apostas.Count;
            var somaApostas = usuario.Apostas.Sum(a => a.Valor);
            var mediaApostas = usuario.Apostas.Any() ? usuario.Apostas.Average(a => a.Valor) : 0;
            var maiorAposta = usuario.Apostas.Any() ? usuario.Apostas.Max(a => a.Valor) : 0;
            var menorAposta = usuario.Apostas.Any() ? usuario.Apostas.Min(a => a.Valor) : 0;

            // LINQ: Apostas agrupadas por resultado
            var apostasPorResultado = usuario.Apostas
                .GroupBy(a => a.Resultado)
                .Select(g => new
                {
                    Resultado = g.Key,
                    Quantidade = g.Count(),
                    ValorTotal = g.Sum(a => a.Valor)
                })
                .ToList();

            // LINQ: Reflexões agrupadas por sentimento
            var reflexoesPorSentimento = usuario.Reflexoes
                .GroupBy(r => r.Sentimento)
                .Select(g => new
                {
                    Sentimento = g.Key,
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Quantidade)
                .ToList();

            var relatorio = new
            {
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.DataCriacao,
                    usuario.Pontos
                },
                EstatisticasApostas = new
                {
                    TotalApostas = totalApostas,
                    SomaTotal = somaApostas,
                    MediaValor = mediaApostas,
                    MaiorAposta = maiorAposta,
                    MenorAposta = menorAposta,
                    ApostasPorResultado = apostasPorResultado
                },
                EstatisticasReflexoes = new
                {
                    TotalReflexoes = usuario.Reflexoes.Count,
                    ReflexoesPorSentimento = reflexoesPorSentimento
                },
                UltimasApostas = usuario.Apostas
                    .OrderByDescending(a => a.Data)
                    .Take(5)
                    .Select(a => new
                    {
                        a.Id,
                        a.Descricao,
                        a.Valor,
                        a.Resultado,
                        a.Data
                    })
                    .ToList(),
                UltimasReflexoes = usuario.Reflexoes
                    .OrderByDescending(r => r.Data)
                    .Take(5)
                    .Select(r => new
                    {
                        r.Id,
                        r.Sentimento,
                        r.Data
                    })
                    .ToList()
            };

            return Ok(relatorio);
        }

        // GET: api/Relatorios/geral
        [HttpGet("geral")]
        public async Task<ActionResult<object>> GetRelatorioGeral()
        {
            // LINQ: Estatísticas gerais do sistema
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalApostas = await _context.Apostas.CountAsync();
            var totalReflexoes = await _context.Reflexoes.CountAsync();

            var somaGeralApostas = await _context.Apostas.SumAsync(a => (decimal?)a.Valor) ?? 0;
            var mediaGeralApostas = await _context.Apostas.AnyAsync()
                ? await _context.Apostas.AverageAsync(a => a.Valor)
                : 0;

            // LINQ: Top 5 usuários com mais apostas
            var topUsuariosApostas = await _context.Usuarios
                .Select(u => new
                {
                    u.Id,
                    u.Nome,
                    TotalApostas = u.Apostas.Count,
                    ValorTotal = u.Apostas.Sum(a => a.Valor)
                })
                .OrderByDescending(x => x.TotalApostas)
                .Take(5)
                .ToListAsync();

            // LINQ: Apostas por mês
            var apostasPorMes = await _context.Apostas
                .GroupBy(a => new { a.Data.Year, a.Data.Month })
                .Select(g => new
                {
                    Ano = g.Key.Year,
                    Mes = g.Key.Month,
                    Quantidade = g.Count(),
                    ValorTotal = g.Sum(a => a.Valor)
                })
                .OrderByDescending(x => x.Ano)
                .ThenByDescending(x => x.Mes)
                .ToListAsync();

            var relatorioGeral = new
            {
                Resumo = new
                {
                    TotalUsuarios = totalUsuarios,
                    TotalApostas = totalApostas,
                    TotalReflexoes = totalReflexoes,
                    SomaGeralApostas = somaGeralApostas,
                    MediaGeralApostas = mediaGeralApostas
                },
                TopUsuarios = topUsuariosApostas,
                ApostasPorMes = apostasPorMes
            };

            return Ok(relatorioGeral);
        }

        // GET: api/Relatorios/apostas/periodo?dataInicio=2024-01-01&dataFim=2024-12-31
        [HttpGet("apostas/periodo")]
        public async Task<ActionResult<object>> GetApostasPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var apostas = await _context.Apostas
                .Where(a => a.Data >= dataInicio && a.Data <= dataFim)
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.Data)
                .Select(a => new
                {
                    a.Id,
                    a.Descricao,
                    a.Valor,
                    a.Resultado,
                    a.Data,
                    Usuario = a.Usuario.Nome
                })
                .ToListAsync();

            var estatisticas = new
            {
                TotalApostas = apostas.Count,
                ValorTotal = apostas.Sum(a => a.Valor),
                ValorMedio = apostas.Any() ? apostas.Average(a => a.Valor) : 0
            };

            return Ok(new
            {
                Periodo = new { DataInicio = dataInicio, DataFim = dataFim },
                Estatisticas = estatisticas,
                Apostas = apostas
            });
        }
    }
}