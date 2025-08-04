using System.Net.Http.Json;
using System.Text.Json;

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
        
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false
        };
        _client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(60)
        };
    }
    
    public async Task CreateCollectionAsync()
    {
        var payload = new
        {
            vectors = new Dictionary<string, object>
            {
                { "size", 768 },
                { "distance", "Cosine" } // ← Büyük harf!
            }
        };

        var url = $"{_baseUrl}/collections/{_collectionName}";

        Console.WriteLine($"[Qdrant] Oluşturma isteği gönderiliyor: {url}");
        Console.WriteLine("Payload:");
        Console.WriteLine(JsonSerializer.Serialize(payload));

        try
        {
            var response = await _client.PutAsJsonAsync(url, payload);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"✅ Collection '{_collectionName}' başarıyla oluşturuldu.");
            }
            else
            {
                Console.WriteLine($"❌ HTTP Hatası: {response.StatusCode}");
                Console.WriteLine($"Yanıt içeriği: {content}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("💥 Qdrant ile bağlantı kurulamadı!");
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }




    public async Task UploadEmbeddingAsync(int id, float[] vector, string text)
    {
        var payload = new
        {
            points = new[]
            {
                new
                {
                    id = id, // 🔥 artık sade integer
                    vector = vector,
                    payload = new { text = text }
                }
            }
        };

        var url = $"{_baseUrl}/collections/{_collectionName}/points?wait=true";

        Console.WriteLine($"[Qdrant] Uploading id: {id}, vector length: {vector.Length}");

        var response = await _client.PutAsJsonAsync(url, payload);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ Yükleme başarısız. Kod: {response.StatusCode}");
            Console.WriteLine($"Yanıt içeriği: {content}");
            throw new HttpRequestException($"Qdrant'a yükleme başarısız: {response.StatusCode}");
        }
    }

    }
}
