#pragma warning disable SKEXP0001

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.Qdrant;


namespace SemanticKernelRAG
{
    public class QdrantMemoryHelper
    {
        private const string CollectionName = "kararlar";

        private readonly Kernel _kernel;
        private readonly ISemanticTextMemory _memory;

        public QdrantMemoryHelper(Kernel kernel)
        {
            _kernel = kernel;

            // Create Qdrant memory store
            var qdrant = new QdrantMemoryStore(
                endpoint: "http://192.168.1.10:7000", // your Qdrant endpoint
                vectorSize: 768                         // depends on your embed model
            );

            // Register memory with embedding model
            _memory = new MemoryBuilder()
                .WithMemoryStore(qdrant)
                .WithOllamaTextEmbeddingGeneration("nomic-embed-text", "http://192.168.137.182:11434")
                .Build();
        }

        // Add a legal case to vector DB
        public async Task SaveCaseAsync(string id, string caseText)
        {
            await _memory.SaveInformationAsync(
                collection: CollectionName,
                text: caseText,
                id: id
            );
        }

        // Search for relevant legal cases
        public async Task<string> SearchCasesAsync(string query)
        {
            var results = await _memory.SearchAsync(CollectionName, query, limit: 3, minRelevanceScore: 0.7);

            var resultBuilder = new System.Text.StringBuilder();
            foreach (var result in results)
            {
                resultBuilder.AppendLine(result.Metadata.Text);
                resultBuilder.AppendLine("\n---\n");
            }

            return resultBuilder.ToString();
        }
    }
}
