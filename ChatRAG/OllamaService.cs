using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace RAGPipeline
{
    public class OllamaService
    {
        private readonly HttpClient _client = new();

        private readonly List<object> _messages = new();

        public OllamaService()
        {
            _messages.Add(new
            {
                role = "system",
                content = @"Sen bir hukuk danışmanı değil, sadece JSON çıktısı sağlayan bir sınıflandırma sistemisin.
            Kullanıcının verdiği olayı sadece aşağıdaki JSON formatında değerlendir:

            {
            ""suç_türü"": [""""],
            ""ceza_türü"": """",
            ""ceza_aralığı_yıl"": {
                ""min"": 0,
                ""max"": 0
            },
            ""tahrik"": false,
            ""iyi_hal"": false,
            ""meşru_müdafaa"": false,
            ""teşebbüs"": false,
            ""ek_not"": """"
            }

            JSON dışı hiçbir şey yazma."
            });
        }

        public async Task AnswerAsync(string context, string userInput)
        {
            string prompt = $"Aşağıdaki yargı kararlarını dikkate alarak kullanıcı olayına hukuki değerlendirme yap.\n\n{context}\n\nOlay: {userInput}";

            _messages.Add(new { role = "user", content = prompt });

            var requestBody = new
            {
                model = "gemma3:4b",
                messages = _messages,
                stream = true  // streaming modda parça parça gelecek
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync("http://192.168.1.102:11434/api/chat", content);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var fullResponseBuilder = new StringBuilder();

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        using var doc = JsonDocument.Parse(line);
                        if (doc.RootElement.TryGetProperty("message", out var message) &&
                            message.TryGetProperty("content", out var contentProp))
                        {
                            string contentPiece = contentProp.GetString();
                            fullResponseBuilder.Append(contentPiece);
                        }
                    }
                    catch
                    {
                        // Geçici hataları yut
                    }
                }

                string fullResponse = fullResponseBuilder.ToString();
                Console.WriteLine("\n📤 Tam birleştirilmiş LLM cevabı:\n" + fullResponse);

                _messages.Add(new { role = "assistant", content = fullResponse });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Ollama error: " + ex.Message);
            }
        }
    }
}
