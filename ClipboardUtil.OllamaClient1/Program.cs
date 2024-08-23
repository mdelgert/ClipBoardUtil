using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardUtil.OllamaClient1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");

                var jsonPayload = "{\n    \"model\": \"llama2\",\n    \"prompt\": \"Why is the sky blue?\",\n    \"stream\": false\n}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from Ollama:");
                Console.WriteLine(responseContent);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e.Message}");
            }
        }
    }
}
