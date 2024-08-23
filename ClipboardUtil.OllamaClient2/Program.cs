using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClipboardUtil.OllamaClient2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using var client = new HttpClient();

                // Get the environment variable to determine the runtime environment
                string environment = Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER")?.ToLower();

                string url = environment switch
                {
                    "true" => "http://host.docker.internal:11434/api/generate",
                    _ => "http://localhost:11434/api/generate"
                };

                while (true)
                {
                    Console.Write(">>>");
                    string userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.WriteLine("Please enter a valid prompt.");
                        continue;
                    }

                    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Exiting chat. Goodbye!");
                        break;
                    }

                    var model = new
                    {
                        model = "llama2",
                        prompt = userInput,
                        stream = true  // Set stream to true
                    };

                    var jsonPayload = JsonSerializer.Serialize(model);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = content;

                    // Send the request and get the response stream
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    // Process the response stream
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    using var streamReader = new System.IO.StreamReader(responseStream);

                    //Console.Write("Ollama: ");

                    while (!streamReader.EndOfStream)
                    {
                        var line = await streamReader.ReadLineAsync();
                        if (!string.IsNullOrEmpty(line))
                        {
                            var responseModel = JsonSerializer.Deserialize<StreamResponseModel>(line);
                            if (responseModel != null)
                            {
                                Console.Write($"{responseModel.Response}");
                            }
                        }
                    }

                    Console.WriteLine("\nStreaming complete.\n");
                }
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

    // Define a model to represent each streamed response line
    public class StreamResponseModel
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }

        [JsonPropertyName("done_reason")]
        public string DoneReason { get; set; }
    }
}
