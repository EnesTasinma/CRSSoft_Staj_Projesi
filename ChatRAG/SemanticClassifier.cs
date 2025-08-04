using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using RAGPipeline.Models;

namespace RAGPipeline
{
    public class SemanticClassifier
    {
        private readonly HttpClient _client = new();

        public async Task<LegalClassification> ClassifyAsync(string userInput)
        {
            var systemPrompt = @"
Kullanıcının olay anlatımını analiz et ve sadece aşağıdaki JSON formatında yanıt ver:

{
  ""suç_türü"": [""kasten öldürme"", ""haksız tahrik""],
  ""ceza_türü"": ""hapis"",
  ""ceza_aralığı_yıl"": {
    ""min"": 15,
    ""max"": 20
  },
  ""tahrik"": true,
  ""iyi_hal"": false,
  ""meşru_müdafaa"": false,
  ""teşebbüs"": false,
  ""ek_not"": ""Olayda saldırganın davranışı tahrik olarak değerlendirilebilir.""
}

Açıklama ekleme. Sadece JSON döndür.";

            var classificationPrompt = $"Olay: {userInput}";

            var requestBody = new
            {
                model = "gemma3:4b",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = classificationPrompt }
                }
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            

            try
            {
                var response = await _client.PostAsync("http://192.168.1.102:11434/api/chat", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                // Console.WriteLine("\n📤 LLM'den gelen cevap:\n" + responseBody);


                // 🧠 JSON blokunu ayıkla
                string jsonBlock = ExtractJsonBlock(responseBody);

                if (!string.IsNullOrWhiteSpace(jsonBlock))
                {
                    var classification = JsonSerializer.Deserialize<LegalClassification>(jsonBlock);

                    if (classification == null)
                    {
                        Console.WriteLine("❌ Deserialize işlemi başarısız.");
                        return DefaultClassification();
                    }

                    classification.Normalize();
                    return classification;
                }


                return DefaultClassification(); // fallback
            }
            catch
            {
                return DefaultClassification(); // fallback
            }
        }

        private string ExtractJsonBlock(string input)
        {
            // Markdown işaretlemelerini temizle
            input = input.Replace("```json", "").Replace("```", "").Trim();

            int start = input.IndexOf('{');
            int end = input.LastIndexOf('}');
            if (start >= 0 && end > start)
            {
                return input.Substring(start, end - start + 1);
            }
            return null;
        }


        private LegalClassification DefaultClassification()
        {
            return new LegalClassification
            {
                Suclar = new List<string> { "belirsiz" },
                CezaTuru = "bilinmiyor",
                CezaAraligi = new CezaAraligi { Min = 0, Max = 0 },
                Tahrik = false,
                IyiHal = false,
                MesruMudafaa = false,
                Tesebbus = false,
                EkNot = "Değerlendirme yapılamadı."
            };
        }
    }
}