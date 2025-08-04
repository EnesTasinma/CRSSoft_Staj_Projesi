using System.Net.Http.Json;
using System.Text.Json;

namespace Services
{
    public class EmbeddingService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public EmbeddingService(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new HttpClient();
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var payload = new
            {
                model = "nomic-embed-text",
                prompt = text
            };

            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/embeddings", payload);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine("Ollama response content: " + content);

            var result = await response.Content.ReadFromJsonAsync<Models.OllamaEmbeddingResponse>();

            if (result == null || result.embedding == null || result.embedding.Length == 0)
            {
                throw new Exception("Embedding response is null or empty.");
            }

            return result.embedding;

        }
    }
}
