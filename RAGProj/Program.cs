using System;
using System.IO;
using System.Threading.Tasks;
using Services;

class Program
{
    static async Task Main(string[] args)
    {
        var ollamaUrl = "http://10.10.20.62:11434";
        var qdrantUrl = "http://10.10.20.62:7000";
        var collection = "kararlar";

        var embeddingService = new EmbeddingService(ollamaUrl);
        var qdrantService = new QdrantService(qdrantUrl, collection);

        await qdrantService.CreateCollectionAsync();

        int globalId = 0;

        foreach (var file in Directory.GetFiles("TXTs", "*.txt"))
        {
            Console.WriteLine($"İşleniyor: {file}");
            var rawText = TxtProcessor.ExtractTextFromTxt(file);
            var chunks = TextChunker.ChunkText(rawText);

            foreach (var chunk in chunks)
            {
                if (string.IsNullOrWhiteSpace(chunk))
                    continue;

                var embedding = await embeddingService.GetEmbeddingAsync(chunk);
                await qdrantService.UploadEmbeddingAsync(globalId, embedding, chunk);
                Console.WriteLine($"✅ Yüklendi: {globalId}");
                globalId++;
            }
        }

        Console.WriteLine("🚀 Tüm embedding'ler başarıyla yüklendi.");
    }
}
