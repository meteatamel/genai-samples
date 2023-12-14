using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class GenerateTextFromImageLocal
{
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";
    const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
    const string ModelId = "gemini-pro-vision";
    const string EndpointUrl = $"{AiPlatformUrl}/v1beta1/projects/{ProjectId}/locations/{Location}/publishers/google/models/{ModelId}:streamGenerateContent";

    // This sample assumes that you have a local image.jpg file
    // Download an image from Google Cloud Storage
    // gsutil cp "gs://cloud-samples-data/generative-ai/image/320px-Felis_catus-cat_on_snow.jpg" ./image.jpg

    public async static Task Generate()
    {
        string text = "Is it a cat?";
        string imagePath = "image.jpg";
        Console.WriteLine($"Text: {text}");
        Console.WriteLine($"Image: {imagePath}");

        byte[] imageBytes = File.ReadAllBytes(imagePath);
        string imageData = Convert.ToBase64String(imageBytes);

        string payload = GeneratePayload(text, imageData);
        string response = await SendRequest(payload);
        var geminiResponses = JsonConvert.DeserializeObject<List<GeminiResponse>>(response);

        string fullText = string.Join("", geminiResponses
            .SelectMany(response => response.Candidates)
            .SelectMany(candidates => candidates.Content.Parts)
            .Select(part => part.Text));

        Console.WriteLine($"Response: {fullText}");
    }

    private static string GeneratePayload(string text, string imageData)
    {
        var payload = new
        {
            contents = new
            {
                role = "USER",
                parts = new object[] {
                    new {text = text},
                    new {inline_data = new {
                            mime_type = "image/jpeg",
                            data = imageData
                        }
                    }
                }
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

        HttpResponseMessage response = await httpClient.PostAsync(EndpointUrl,
            new StringContent(payload, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}