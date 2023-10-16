using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class Rest
{
    // Adjust for your project
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";

    const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
    const string ModelName = "text-bison";
    const string PredictUrl = $"{AiPlatformUrl}/v1/projects/{ProjectId}/locations/{Location}/publishers/google/models/{ModelName}:predict";

    public async static Task<string> GenerateText(string prompt)
    {
        string payload = GeneratePayload(prompt);
        string response = await SendRequest(payload);
        return response;
    }

    private static string GeneratePayload(string prompt)
    {
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
}