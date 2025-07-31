using System.Net.Http.Json;

namespace Services
{
    public class QdrantService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private readonly string _collectionName;

    public QdrantService(string baseUrl, string collectionName)
    {
        _baseUrl = baseUrl;
        _collectionName = collectionName;
        _client = new HttpClient();
    }

    public async Task UploadEmbeddingAsync(string id, float[] vector, string text)
    {
        var payload = new
        {
            points = new[]
            {
                new
                {
                    id = id,
                    vector = vector,
                    payload = new { text = text }
                }
            }
        };

        var url = $"{_baseUrl}/collections/{_collectionName}/points?wait=true";
        var response = await _client.PutAsJsonAsync(url, payload);
        response.EnsureSuccessStatusCode();
    }
}
}
