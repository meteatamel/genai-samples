using Google.Cloud.AIPlatform.V1;
using Newtonsoft.Json;
using wkt = Google.Protobuf.WellKnownTypes;

public class Library
{
    // Adjust for your project
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";

    const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
    const string Publisher = "google";
    const string ModelName = "imagegeneration";

    public async static Task<List<Image>> GenerateImages(string prompt, int sampleCount)
    {
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
            EndpointAsEndpointName = EndpointName.FromProjectLocationPublisherModel(ProjectId, Location, Publisher, ModelName),
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
}