using System.Net.Http.Json;
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
        var result = await response.Content.ReadFromJsonAsync<Models.OllamaEmbeddingResponse>();
        return result.embeddings[0];
    }
}

}