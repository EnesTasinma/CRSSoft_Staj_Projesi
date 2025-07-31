using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Text;

namespace Services
{
    public class PdfProcessor
{
    public static string ExtractTextFromPdf(string filePath)
    {
        var textBuilder = new StringBuilder();
        using var document = PdfDocument.Open(filePath);
        foreach (var page in document.GetPages())
        {
            textBuilder.AppendLine(page.Text);
        }
        return textBuilder.ToString();
    }
}
}
