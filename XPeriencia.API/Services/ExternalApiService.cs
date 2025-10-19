using System.Text.Json;

namespace XPeriencia.API.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiService> _logger;

        public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Buscar frase motivacional
        public async Task<string> GetFraseMotivacionalAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.quotable.io/random");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                var quote = json.RootElement.GetProperty("content").GetString();
                var author = json.RootElement.GetProperty("author").GetString();

                return $"{quote} - {author}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar frase motivacional");
                return "Mantenha o foco e persista em seus objetivos!";
            }
        }

        // Buscar dados de CEP
        public async Task<object?> GetEnderecoPorCepAsync(string cep)
        {
            try
            {
                // Remove caracteres não numéricos
                cep = new string(cep.Where(char.IsDigit).ToArray());

                if (cep.Length != 8)
                {
                    return null;
                }

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

        // Buscar dados de clima (OpenMeteo - API pública sem necessidade de chave)
        public async Task<object?> GetClimaAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

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