using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

public class Program
{
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";

    const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
    const string ModelName = "imagegeneration";
    const string PredictUrl = $"{AiPlatformUrl}/v1/projects/{ProjectId}/locations/{Location}/publishers/google/models/{ModelName}:predict";

    private async static Task<List<Image>> GenerateImages(string prompt, int sampleCount)
    {
        string payload = GeneratePayload(prompt, sampleCount);
        JObject? jsonObject = await SendRequest(payload);
        List<Image> images = [];

        JArray? predictions = jsonObject?["predictions"] as JArray;
        if (predictions != null)
        {
            foreach (JObject prediction in predictions)
            {
                byte[] bytesBase64Encoded = Convert.FromBase64String(prediction["bytesBase64Encoded"].ToString());
                using MemoryStream memoryStream = new(bytesBase64Encoded);
                Image image = Image.Load(memoryStream);
                images.Add(image);
            }
        }
        else
        {
            Console.WriteLine($"No predictions for prompt: {prompt}");
        }

        return images;
    }

    private static string GeneratePayload(string prompt, int sampleCount)
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
                sampleCount = sampleCount,
            }
        };

        return JsonConvert.SerializeObject(payload);
    }

    private async static Task<JObject?> SendRequest(string payload)
    {
        GoogleCredential credential = GoogleCredential.GetApplicationDefault();
        var handler = credential.ToDelegatingHandler(new HttpClientHandler());
        using HttpClient httpClient = new(handler);

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await httpClient.PostAsync(PredictUrl,
            new StringContent(payload, Encoding.UTF8, "application/json"));

        string contentString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<JObject>(contentString);
    }

    private static async Task SaveImages(List<Image> images, string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        for (int idx = 0; idx < images.Count; idx++)
        {
            string imgPath = Path.Combine(folder, $"image_{idx}.png");

            using FileStream fs = File.OpenWrite(imgPath);
            await images[idx].SaveAsPngAsync(fs);
            Console.WriteLine($"Saved {imgPath}");
        }
    }


    static async Task Main(string[] args)
    {
        var images = await GenerateImages("happy dogs", 2);
        await SaveImages(images, "images");
    }
}