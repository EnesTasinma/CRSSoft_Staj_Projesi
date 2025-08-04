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

            Console.WriteLine("\n🧠 Tespit edilen sınıflar:");
            classification.Suclar.ForEach(c => Console.WriteLine($"- {c}"));
            Console.WriteLine($"Ceza Türü: {classification.CezaTuru}");
            Console.WriteLine($"Ceza Aralığı: {classification.CezaAraligi.Min} - {classification.CezaAraligi.Max} yıl");
            Console.WriteLine($"Ek Not: {classification.EkNot}");

            String combinedQuery = "";
            // 2. Bu sınıfları tek cümle haline getir
            if (classification.Suclar.Count == 0 || classification.Suclar.Contains("belirsiz"))
            {
                Console.WriteLine("⚠️ Hukuki sınıflandırma yapılamadı.");
            }
            else
            {
                combinedQuery = string.Join(" ve ", classification.Suclar) + " ile ilgili Yargıtay kararları";
                Console.WriteLine($"🔎 Arama ifadesi: {combinedQuery}");
            }


            // 3. Embedding al
            var embedder = new EmbeddingService();
            var embedding = await embedder.GetEmbedding(combinedQuery);
            if (embedding == null) return;

            // 4. Qdrant'tan karar çek
            var qdrant = new QdrantService();
            var context = await qdrant.Search(embedding);

            // 5. LLM ile cevap üret
            var llm = new OllamaService();
            await llm.AnswerAsync(context, question);
        }
    }
}
