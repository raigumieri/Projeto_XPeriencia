using System.Text.Json;

namespace XPeriencia.API.Services
{
    /// <summary>
    /// Serviço responsável por integração com APIs externas.
    /// Fornece métodos para buscar dados de diferentes fontes públicas.
    /// </summary>
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiService> _logger;

        /// <summary>
        /// Construtor com injeção de dependências.
        /// HttpClient é injetado automaticamente pelo ASP.NET Core.
        /// </summary>
        public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Busca uma frase motivacional aleatória da API Quotable.
        /// Utilizada para fornecer inspiração aos usuários.
        /// </summary>
        /// <returns>String com a frase e autor, ou mensagem padrão em caso de erro</returns>
        public async Task<string> GetFraseMotivacionalAsync()
        {
            try
            {
                // Requisição GET para a API Quotable
                var response = await _httpClient.GetAsync("https://api.quotable.io/random");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                // Extrai a frase e o autor do JSON retornado
                var quote = json.RootElement.GetProperty("content").GetString();
                var author = json.RootElement.GetProperty("author").GetString();

                return $"{quote} - {author}";
            }
            catch (Exception ex)
            {
                // Log do erro e retorno de mensagem padrão
                _logger.LogError(ex, "Erro ao buscar frase motivacional");
                return "Mantenha o foco e persista em seus objetivos!";
            }
        }

        /// <summary>
        /// Consulta dados de endereço através do CEP usando a API ViaCEP.
        /// Remove caracteres não numéricos e valida o formato do CEP.
        /// </summary>
        /// <param name="cep">CEP a ser consultado (com ou sem formatação)</param>
        /// <returns>Objeto com dados do endereço ou null se inválido/não encontrado</returns>
        public async Task<object?> GetEnderecoPorCepAsync(string cep)
        {
            try
            {
                // Sanitização: remove tudo que não é dígito
                cep = new string(cep.Where(char.IsDigit).ToArray());

                // Validação: CEP brasileiro deve ter exatamente 8 dígitos
                if (cep.Length != 8)
                {
                    return null;
                }

                // Requisição para a API ViaCEP
                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var endereco = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

                return endereco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar CEP");
                return null;
            }
        }

        /// <summary>
        /// Busca dados meteorológicos em tempo real usando a API OpenMeteo.
        /// Não requer chave de API, é totalmente pública e gratuita.
        /// </summary>
        /// <param name="latitude">Latitude da localização</param>
        /// <param name="longitude">Longitude da localização</param>
        /// <returns>Objeto com temperatura, vento e horário, ou null em caso de erro</returns>
        public async Task<object?> GetClimaAsync(double latitude, double longitude)
        {
            try
            {
                // Monta a URL com os parâmetros de localização
                var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                // Extrai apenas os dados meteorológicos atuais
                var currentWeather = json.RootElement.GetProperty("current_weather");

                return new
                {
                    Temperatura = currentWeather.GetProperty("temperature").GetDouble(),
                    VelocidadeVento = currentWeather.GetProperty("windspeed").GetDouble(),
                    DirecaoVento = currentWeather.GetProperty("winddirection").GetDouble(),
                    Horario = currentWeather.GetProperty("time").GetString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados de clima");
                return null;
            }
        }
    }
}