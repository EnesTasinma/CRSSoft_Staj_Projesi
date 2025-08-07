using System;
using System.IO;
using System.Threading.Tasks;
using Services;

namespace SemanticKernelRAG
{
    public static class Seeder
    {
        public static async Task SeedFromFolderAsync(QdrantMemoryHelper memory, string folder = "TXTs")
        {
            int globalId = 0;

            foreach (var file in Directory.GetFiles(folder, "*.txt"))
            {
                Console.WriteLine($"📂 İşleniyor: {file}");

                var rawText = TxtProcessor.ExtractTextFromTxt(file);
                var chunks = TextChunker.ChunkText(rawText);

                foreach (var chunk in chunks)
                {
                    if (string.IsNullOrWhiteSpace(chunk))
                        continue;

                    string id = $"chunk-{globalId}";
                    await memory.SaveCaseAsync(id, chunk);

                    Console.WriteLine($"✅ Yüklendi: {id}");
                    globalId++;
                }
            }

            Console.WriteLine("🚀 Embedding yükleme tamamlandı.");
        }
    }
}
