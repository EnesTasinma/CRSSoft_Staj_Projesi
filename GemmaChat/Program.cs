using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string OllamaApiUrl = "http://192.168.1.65:11434/api/generate";
    private static readonly List<string> chatHistory = new List<string>();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to Gemma Chat! Type your message and press Enter. Type 'exit' to quit.");
        Console.WriteLine("Using model: gemma3:4b.");

        while (true)
        {
            Console.Write("> ");
            string userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput) || userInput.ToLower() == "exit")
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            try
            {
                chatHistory.Add($"User: {userInput}");
                string response = await SendMessageToGemma(userInput);
                chatHistory.Add($"Gemma: {response}");
                Console.WriteLine("Gemma: " + response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static async Task<string> SendMessageToGemma(string message)
    {
        // Combine chat history with the new message
        string prompt = string.Join("\n", chatHistory) + $"\nUser: {message}\nGemma: ";

        var requestBody = new
        {
            model = "gemma3:4b", // Replace with "gemma3:4b" if available
            prompt = prompt,
            stream = false
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(OllamaApiUrl, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var jsonResponse = JObject.Parse(responseString);

        return jsonResponse["response"]?.ToString() ?? "No response from Gemma.";
    }
}