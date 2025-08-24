using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Encodings.Web;

namespace RAGPipeline
{
    public class QdrantService
    {
        private readonly HttpClient _client = new();

        public async Task<string> Search(float[] vector)
        {
            var requestBody = new
            {
                vector = vector,
                top = 3,
                with_payload = true
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync("http://192.168.1.4:7000/collections/kararlar/points/search", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var results = doc.RootElement.GetProperty("result");

                var contextBuilder = new System.Text.StringBuilder();

                foreach (var result in results.EnumerateArray())
                {
                    if (result.TryGetProperty("payload", out var payload) &&
                        payload.TryGetProperty("text", out var text))
                    {
                        contextBuilder.AppendLine(text.GetString());
                        contextBuilder.AppendLine("\n---\n");
                    }
                }

                return contextBuilder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Qdrant error: " + ex.Message);
                return "";
            }
        }
    }
}
