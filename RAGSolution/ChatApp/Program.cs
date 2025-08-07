using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SemanticKernelRAG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Build the Semantic Kernel
            var builder = Kernel.CreateBuilder();

            // Use your remote Ollama server
            builder.AddOllamaChatCompletion(
                modelId: "gemma3:4b",
                endpoint: new Uri("http://192.168.1.10:11434") // Convert string to Uri
            );

            var kernel = builder.Build();

            // 2. Ask the user for input
            Console.Write("Olayı yazın: ");
            string userInput = Console.ReadLine();

            // 3. Run the classification step (we will build this function next!)
            var classificationFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate: SemanticPrompts.ClassifierPrompt,
                functionName: "ClassifyCrime",
                description: "Hukuki sınıflandırma yapar."
            );

            var classificationResult = await kernel.InvokeAsync(classificationFunction, new() { ["input"] = userInput });

            Console.WriteLine("\n🧠 LLM'den sınıflandırma çıktısı:\n" + classificationResult);

            // 4. Search for similar cases using Qdrant
            var memory = new QdrantMemoryHelper(kernel);
            var searchResults = await memory.SearchCasesAsync(userInput);

            if (string.IsNullOrWhiteSpace(searchResults))
            {
                Console.WriteLine("⚠️ Qdrant sonuç döndüremedi.");
                return;
            }

            Console.WriteLine("\n📚 Bulunan kararlar:\n" + searchResults);

            // 5. Generate final legal classification using LLM + context
            var finalResponseFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate: SemanticPrompts.FinalResponsePrompt,
                functionName: "FinalEvaluation",
                description: "Yargı kararlarını kullanarak olay değerlendirmesi yapar."
            );

            var finalResult = await kernel.InvokeAsync(finalResponseFunction, new()
            {
                ["context"] = searchResults,
                ["input"] = userInput
            });

            Console.WriteLine("\n📤 Nihai LLM JSON cevabı:\n" + finalResult);

            var finalResultText = finalResult.ToString();
            try
            {
                var classification = JsonSerializer.Deserialize<LegalClassification>(finalResultText);
                classification?.Normalize();

                Console.WriteLine("\n📋 Yapılandırılmış JSON Nesnesi:");
                Console.WriteLine(classification);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ JSON parse hatası: " + ex.Message);
                Console.WriteLine("Gelen içerik:\n" + finalResultText);
            }


        }
    }
}
