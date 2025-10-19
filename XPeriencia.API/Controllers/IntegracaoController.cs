using Microsoft.AspNetCore.Mvc;
using XPeriencia.API.Services;

namespace XPeriencia.API.Controllers
{
    /// <summary>
    /// Controller responsável por integração com APIs externas.
    /// Fornece endpoints que consomem serviços de terceiros para enriquecer a experiência do usuário.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IntegracaoController : ControllerBase
    {
        private readonly ExternalApiService _externalApiService;

        /// <summary>
        /// Construtor com injeção de dependência do serviço de APIs externas.
        /// </summary>
        public IntegracaoController(ExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        /// <summary>
        /// Retorna uma frase motivacional aleatória da API Quotable.
        /// Útil para inspirar usuários em sua jornada de combate ao vício.
        /// </summary>
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

        /// <summary>
        /// Consulta dados de endereço através do CEP usando ViaCEP.
        /// Pode ser usado para completar cadastros de usuários.
        /// </summary>
        /// <param name="cep">CEP a ser consultado (com ou sem formatação)</param>
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

        /// <summary>
        /// Retorna dados meteorológicos em tempo real para uma localização.
        /// Usa coordenadas geográficas (latitude e longitude).
        /// Exemplo: São Paulo = latitude -235505, longitude -466333
        /// </summary>
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

        /// <summary>
        /// Endpoint que combina dados do usuário com frase motivacional externa.
        /// Demonstra integração entre dados locais e APIs externas.
        /// </summary>
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