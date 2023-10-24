using Google.Cloud.AIPlatform.V1;
using Newtonsoft.Json;
using wkt = Google.Protobuf.WellKnownTypes;

public class Program
{
    static readonly string? ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");
    static readonly string? Region = Environment.GetEnvironmentVariable("REGION");

    static readonly string AiPlatformUrl = $"https://{Region}-aiplatform.googleapis.com";
    const string Publisher = "google";
    const string ModelName = "imagegeneration";

    public async static Task<List<Image>> GenerateImages(string prompt, int sampleCount)
    {
        if (string.IsNullOrEmpty(ProjectId) || string.IsNullOrEmpty(Region))
        {
            throw new Exception("Environment variable 'PROJECT_ID' or 'REGION' not set.");
        }

        var instance = new
        {
            prompt = prompt
        };

        var parameters = new
        {
            sampleCount = sampleCount
        };

        PredictionServiceClient client = await new PredictionServiceClientBuilder
        {
            Endpoint = AiPlatformUrl
        }.BuildAsync();

        var request = new PredictRequest
        {
            EndpointAsEndpointName = EndpointName.FromProjectLocationPublisherModel(ProjectId, Region, Publisher, ModelName),
            Instances = { wkt::Value.Parser.ParseJson(JsonConvert.SerializeObject(instance)) },
            Parameters = wkt::Value.Parser.ParseJson(JsonConvert.SerializeObject(parameters)),
        };

        var response = await client.PredictAsync(request);
        List<Image> images = [];

        if (response.Predictions != null)
        {
            foreach (var prediction in response.Predictions)
            {
                byte[] bytesBase64Encoded = Convert.FromBase64String(prediction.StructValue.Fields["bytesBase64Encoded"].StringValue);
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

    static async Task Main()
    {
        var images = await GenerateImages("happy dogs", 2);
        await SaveImages(images, "images");
    }
}