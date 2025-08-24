using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace RAGPipeline
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Olayı yazın: ");
            string question = Console.ReadLine();

            // 1. Semantik sınıflandırma (kasten öldürme, tahrik vs.)
            var classifier = new SemanticClassifier();
            var classification = await classifier.ClassifyAsync(question);

            //Console.WriteLine("\n🧠 Tespit edilen sınıflar:");
            //classification.Crimes.ForEach(c => Console.WriteLine($"- {c}"));
            //Console.WriteLine($"Ceza Türü: {classification.PenaltyType}");
            //Console.WriteLine($"Ceza Aralığı: {classification.PenaltyRange.Min} - {classification.PenaltyRange.Max} yıl");
            //Console.WriteLine($"Ek Not: {classification.AdditionalNote}");

            String combinedQuery = "";
            // 2. Bu sınıfları tek cümle haline getir
            if (classification.Crimes.Count == 0 || classification.Crimes.Contains("belirsiz"))
            {
                Console.WriteLine("⚠️ Hukuki Sınıflandırma Yapılamadı!");
                combinedQuery = question;
            }
            else
            {
                combinedQuery = string.Join(" ", classification.Crimes);
                Console.WriteLine($"🔎 Arama ifadesi: {combinedQuery}");
            }


            // 3. Embedding al
            var embedder = new EmbeddingService();
            var embedding = await embedder.GetEmbedding(combinedQuery);
            if (embedding == null) return;

            // 4. Qdrant'tan karar çek
            var qdrant = new QdrantService();
            var context = await qdrant.Search(embedding);
            if (string.IsNullOrWhiteSpace(context))
            {
                Console.WriteLine("⚠️ Qdrant sonuç döndüremedi. Sorgu ile veri eşleşmiyor.");
                return;
            }
            else
            {
                Console.WriteLine("🎯 Qdrant sonuçları bulundu:\n" + context);
            }


            // 5. LLM ile cevap üret
            var llm = new OllamaService();
            await llm.AnswerAsync(context, question);
        }
    }
}
