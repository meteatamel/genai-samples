using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class Program
{
    static readonly string? ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");
    static readonly string? Region = Environment.GetEnvironmentVariable("REGION");

    static readonly string AiPlatformUrl = $"https://{Region}-aiplatform.googleapis.com";
    const string ModelName = "text-bison";
    static readonly string PredictUrl = $"{AiPlatformUrl}/v1/projects/{ProjectId}/locations/{Region}/publishers/google/models/{ModelName}:predict";

    private async static Task<string> GenerateText(string prompt)
    {
        if (string.IsNullOrEmpty(ProjectId) || string.IsNullOrEmpty(Region))
        {
            throw new Exception("Environment variable 'PROJECT_ID' or 'REGION' not set.");
        }

        string payload = GeneratePayload(prompt);
        string response = await SendRequest(payload);
        Console.WriteLine("Response: " + response);

        dynamic? responseJson = JsonConvert.DeserializeObject(response);
        string content = responseJson.predictions[0].content;
        return content;
    }

    private static string GeneratePayload(string prompt)
    {
        // Get payload
        var payload = new
        {
            instances = new[]
            {
                new
                {
                    prompt = prompt
                }
            },
            parameters = new
            {
                temperature = 0.2,
                maxOutputTokens = 256,
                topK = 40,
                topP = 0.95
            }
        };
        return JsonConvert.SerializeObject(payload);
    }

    private async static Task<string> SendRequest(string payload)
    {
        GoogleCredential credential = GoogleCredential.GetApplicationDefault();
        var handler = credential.ToDelegatingHandler(new HttpClientHandler());
        using HttpClient httpClient = new(handler);

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await httpClient.PostAsync(PredictUrl,
            new StringContent(payload, Encoding.UTF8, "application/json"));

        string content = await response.Content.ReadAsStringAsync();
        return content;
    }

    static async Task Main()
    {
        string prompt = "Give me ten interview questions for the role of program manager.";
        var content = await GenerateText(prompt);
        Console.WriteLine($"Content: {content}");
    }
}