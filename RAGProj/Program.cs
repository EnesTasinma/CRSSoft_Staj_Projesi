var ollamaUrl = "http://192.168.1.47:11434";
var qdrantUrl = "http://192.168.1.47:7001";
var collection = "kararlar";

var embeddingService = new EmbeddingService(ollamaUrl);
var qdrantService = new QdrantService(qdrantUrl, collection);

foreach (var file in Directory.GetFiles("PDFs", "*.pdf"))
{
    Console.WriteLine($"İşleniyor: {file}");
    var rawText = PdfProcessor.ExtractTextFromPdf(file);
    var chunks = TextChunker.ChunkText(rawText);

    int i = 0;
    foreach (var chunk in chunks)
    {
        var embedding = await embeddingService.GetEmbeddingAsync(chunk);
        var id = $"{Path.GetFileNameWithoutExtension(file)}_{i++}";
        await qdrantService.UploadEmbeddingAsync(id, embedding, chunk);
        Console.WriteLine($"Yüklendi: {id}");
    }
}
