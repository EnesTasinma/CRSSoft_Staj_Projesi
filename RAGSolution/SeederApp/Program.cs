using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using System;
using System.IO;
using System.Threading.Tasks;
using Services;
using SemanticKernelRAG;

class Program
{
    static async Task Main(string[] args)
    {
        var ollamaUrl = "http://192.168.1.10:11434";
        var qdrantUrl = "http://192.168.1.10:7000";
        var collection = "kararlar";

        // 1. Set up Semantic Kernel
        var builder = Kernel.CreateBuilder();
        builder.AddOllamaTextEmbeddingGeneration(
            modelId: "nomic-embed-text",
            endpoint: new Uri(ollamaUrl) // Convert string to Uri
        );
        var kernel = builder.Build();

        // 2. Set up memory store
        var qdrantHelper = new QdrantMemoryHelper(kernel);


        await Seeder.SeedFromFolderAsync(qdrantHelper, "TXTs");

        Console.WriteLine("🚀 Tüm embedding'ler başarıyla yüklendi.");
    }
}
