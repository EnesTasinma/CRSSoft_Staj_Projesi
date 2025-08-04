using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RAGPipeline.Models
{
    public class LegalClassification
    {
        [JsonPropertyName("suç_türü")]
        public List<string> Suclar { get; set; }

        [JsonPropertyName("ceza_türü")]
        public string CezaTuru { get; set; }

        [JsonPropertyName("ceza_aralığı_yıl")]
        public CezaAraligi CezaAraligi { get; set; }

        [JsonPropertyName("tahrik")]
        public bool Tahrik { get; set; }

        [JsonPropertyName("iyi_hal")]
        public bool IyiHal { get; set; }

        [JsonPropertyName("meşru_müdafaa")]
        public bool MesruMudafaa { get; set; }

        [JsonPropertyName("teşebbüs")]
        public bool Tesebbus { get; set; }

        [JsonPropertyName("ek_not")]
        public string EkNot { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(CezaTuru))
                CezaTuru = "bilinmiyor";

            if (Suclar == null || Suclar.Count == 0 || string.IsNullOrWhiteSpace(Suclar[0]))
                Suclar = new List<string> { "belirsiz" };

            if (CezaAraligi == null)
                CezaAraligi = new CezaAraligi { Min = 0, Max = 0 };

            if (string.IsNullOrWhiteSpace(EkNot))
                EkNot = "Değerlendirme yapılamadı.";
        }
    }

    public class CezaAraligi
    {
        [JsonPropertyName("min")]
        public int Min { get; set; }

        [JsonPropertyName("max")]
        public int Max { get; set; }
    }
}
