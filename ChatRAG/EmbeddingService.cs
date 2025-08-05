using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RAGPipeline
{
    public class EmbeddingService
    {
        private readonly HttpClient _client = new();

        public async Task<float[]> GetEmbedding(string text)
        {
            var requestBody = new
            {
                model = "nomic-embed-text",
                prompt = text
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync("http://192.168.1.103:11434/api/embeddings", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var embeddingArray = doc.RootElement.GetProperty("embedding");

                float[] vector = new float[embeddingArray.GetArrayLength()];
                int i = 0;
                foreach (var num in embeddingArray.EnumerateArray())
                {
                    vector[i++] = num.GetSingle();
                }

                return vector;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Embedding error: " + ex.Message);
                return null;
            }
        }
    }
}
