using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SemanticKernelRAG.Models
{
    public class LegalClassification
    {
        [JsonPropertyName("suç_türü")]
        public List<string> Crimes { get; set; }

        [JsonPropertyName("ceza_türü")]
        public string PenaltyType { get; set; }

        [JsonPropertyName("ceza_aralığı_yıl")]
        public PenaltyRange PenaltyRange { get; set; }

        [JsonPropertyName("tahrik")]
        public bool Provocation { get; set; }

        [JsonPropertyName("iyi_hal")]
        public bool GoodBehavior { get; set; }

        [JsonPropertyName("meşru_müdafaa")]
        public bool SelfDefense { get; set; }

        [JsonPropertyName("teşebbüs")]
        public bool Attempt { get; set; }

        [JsonPropertyName("ek_not")]
        public string AdditionalNote { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(PenaltyType))
                PenaltyType = "bilinmiyor";

            if (Crimes == null || Crimes.Count == 0 || string.IsNullOrWhiteSpace(Crimes[0]))
                Crimes = new List<string> { "belirsiz" };

            if (PenaltyRange == null)
                PenaltyRange = new PenaltyRange { Min = 0, Max = 0 };

            if (string.IsNullOrWhiteSpace(AdditionalNote))
                AdditionalNote = "Değerlendirme yapılamadı.";
        }

        public override string ToString()
        {
            return $@"
            🧾 Suç Türü: {string.Join(", ", Crimes)}
            🏷️ Ceza Türü: {PenaltyType}
            📅 Ceza Aralığı: {PenaltyRange.Min} - {PenaltyRange.Max} yıl
            ⚠️ Tahrik: {Provocation}
            ✅ İyi Hal: {GoodBehavior}
            🛡️ Meşru Müdafaa: {SelfDefense}
            🔁 Teşebbüs: {Attempt}
            🗒️ Ek Not: {AdditionalNote}
            ";
        }
    }

    public class PenaltyRange
    {
        [JsonPropertyName("min")]
        public int Min { get; set; }

        [JsonPropertyName("max")]
        public int Max { get; set; }
    }
}
