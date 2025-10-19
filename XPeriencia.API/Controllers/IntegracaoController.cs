using Microsoft.AspNetCore.Mvc;
using XPeriencia.API.Services;

namespace XPeriencia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntegracaoController : ControllerBase
    {
        private readonly ExternalApiService _externalApiService;

        public IntegracaoController(ExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        // GET: api/Integracao/frase-motivacional
        [HttpGet("frase-motivacional")]
        public async Task<ActionResult<object>> GetFraseMotivacional()
        {
            var frase = await _externalApiService.GetFraseMotivacionalAsync();

            return Ok(new
            {
                Mensagem = "Frase motivacional para voc�!",
                Frase = frase,
                DataHora = DateTime.Now
            });
        }

        // GET: api/Integracao/cep/01310100
        [HttpGet("cep/{cep}")]
        public async Task<ActionResult<object>> GetEnderecoPorCep(string cep)
        {
            var endereco = await _externalApiService.GetEnderecoPorCepAsync(cep);

            if (endereco == null)
            {
                return NotFound(new { Mensagem = "CEP n�o encontrado ou inv�lido." });
            }

            return Ok(new
            {
                Mensagem = "Endere�o encontrado com sucesso!",
                Dados = endereco
            });
        }

        // GET: api/Integracao/clima?latitude=-23.5505&longitude=-46.6333
        [HttpGet("clima")]
        public async Task<ActionResult<object>> GetClima([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var clima = await _externalApiService.GetClimaAsync(latitude, longitude);

            if (clima == null)
            {
                return NotFound(new { Mensagem = "N�o foi poss�vel obter dados de clima." });
            }

            return Ok(new
            {
                Mensagem = "Dados de clima obtidos com sucesso!",
                Localizacao = new { Latitude = latitude, Longitude = longitude },
                Clima = clima
            });
        }

        // GET: api/Integracao/motivacao-para-usuario/5
        [HttpGet("motivacao-para-usuario/{usuarioId}")]
        public async Task<ActionResult<object>> GetMotivacaoParaUsuario(int usuarioId)
        {
            var frase = await _externalApiService.GetFraseMotivacionalAsync();

            return Ok(new
            {
                UsuarioId = usuarioId,
                Mensagem = "Continue firme em seus objetivos!",
                FraseMotivacional = frase,
                DataHora = DateTime.Now
            });
        }
    }
}