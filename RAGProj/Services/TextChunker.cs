using System.Text;

namespace Services
{
    public class TextChunker
{
    public static List<string> ChunkText(string text, int maxChunkSize = 1000)
    {
        var chunks = new List<string>();
        var words = text.Split(' ');
        var currentChunk = new StringBuilder();

        foreach (var word in words)
        {
            if ((currentChunk.Length + word.Length + 1) > maxChunkSize)
            {
                chunks.Add(currentChunk.ToString());
                currentChunk.Clear();
            }
            currentChunk.Append(word).Append(' ');
        }

        if (currentChunk.Length > 0)
            chunks.Add(currentChunk.ToString());

        return chunks;
    }
}

}