using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

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

                var model = new
                {
                    model = "llama2",
                    prompt = "Why is the sky blue?",
                    stream = false
                };

                Console.WriteLine($"Question: {model.prompt}");

                var jsonPayload = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a ResponseModel object
                var responseModel = JsonSerializer.Deserialize<ResponseModel>(responseContent);

                //Console.WriteLine("Response from Ollama:");

                if (responseModel != null)
                {
                    //Console.WriteLine($"Model: {responseModel.Model}");
                    //Console.WriteLine($"Created At: {responseModel.CreatedAt}");
                    Console.WriteLine($"Response: {responseModel.Response}");
                    //Console.WriteLine($"Done: {responseModel.Done}");
                    //Console.WriteLine($"Done Reason: {responseModel.DoneReason}");
                    //Console.WriteLine($"Total Duration: {responseModel.TotalDuration}");
                    //Console.WriteLine($"Load Duration: {responseModel.LoadDuration}");
                    //Console.WriteLine($"Prompt Eval Duration: {responseModel.PromptEvalDuration}");
                    //Console.WriteLine($"Eval Count: {responseModel.EvalCount}");
                    //Console.WriteLine($"Eval Duration: {responseModel.EvalDuration}");
                    //Console.WriteLine($"Context: {string.Join(", ", responseModel.Context)}");
                }
                else
                {
                    Console.WriteLine("Failed to deserialize response.");
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

    // Define a model to represent the response with correct JSON property names
    public class ResponseModel
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("response")]
        public string Response { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }

        [JsonPropertyName("done_reason")]
        public string DoneReason { get; set; }

        [JsonPropertyName("context")]
        public List<int> Context { get; set; }

        [JsonPropertyName("total_duration")]
        public long TotalDuration { get; set; }

        [JsonPropertyName("load_duration")]
        public long LoadDuration { get; set; }

        [JsonPropertyName("prompt_eval_duration")]
        public long PromptEvalDuration { get; set; }

        [JsonPropertyName("eval_count")]
        public int EvalCount { get; set; }

        [JsonPropertyName("eval_duration")]
        public long EvalDuration { get; set; }
    }
}
