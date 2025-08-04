using System.IO;

namespace Services
{
    public class TxtProcessor
    {
        public static string ExtractTextFromTxt(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
